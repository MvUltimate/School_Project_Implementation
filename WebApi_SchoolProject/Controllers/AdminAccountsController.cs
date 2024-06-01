using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using DAL.Models;
using WebApi_SchoolProject.Services;
using WebApi_SchoolProject.Models;

using Microsoft.AspNetCore.Authorization;
using System.Drawing;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Routing;

namespace WebApi_SchoolProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly AccountService _accountService;
        private readonly StudentService _studentService;
        private readonly TransactionManagerService _transactionManagerService;

        public AdminAccountsController
            (SchoolContext context,
            AccountService accountService,
            StudentService studentService,
            TransactionManagerService transactionManagerService
            )
        {
            _context = context;
            _accountService = accountService;
            _studentService = studentService;
            _transactionManagerService = transactionManagerService;
        }

        [HttpPost("createaccount {Uuid}")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                _accountService.CreateUser(request.UUID, request.Password);
                return Ok("Utilisateur créé avec succès !");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);

            }
        }

        public class CreateUserRequest
        {
            public Guid UUID { get; set; }
            public string Password { get; set; }
        }

        [HttpPost("chargeClass")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task <ActionResult> ChargeClass([FromBody] ChargeClassRequest chargeClassRequest)
        {

            var uuid = User.FindFirst("UUID");
            var senderuuid = new Guid(uuid.Value);

            var studentClass = await _context.Classes.FirstOrDefaultAsync(c => c.Name == chargeClassRequest.Class);
            if (studentClass == null)
            {
                return NotFound("This class doesn't exist");
            }

            var listOfStudents = await _context.SAPs
            .Where(s => s.ClassId == studentClass.ClassId)
            .Include(s => s.Class) // Inclure la classe associée
            .ToListAsync();

            foreach (var student in listOfStudents)
            {
                if (student.Class.Name == studentClass.Name)
                {
                    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UUID == student.UUID);
                    await _transactionManagerService.AddCredit(account, chargeClassRequest.Amount);
                    await _transactionManagerService.WriteTransaction(account.UUID, senderuuid, chargeClassRequest.Amount);
                }
            }

            return Ok("Transaction successfull");

        }

        public class ChargeClassRequest
        {
            public double Amount { get; set; }
            public string Class { get; set; }
        }

        // GET: api/Accounts
        [HttpGet]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<IEnumerable<StudentsInfoM>>> GetAccounts()
        {

            var listOfStudents = await _context.Accounts.ToListAsync();
            List<StudentsInfoM> listOfStudentsInfos = new List<StudentsInfoM>();
            foreach (var student in listOfStudents)
            {
                var studentInfo = await _studentService.GetUsersInfo(student.UUID);
                listOfStudentsInfos.Add(studentInfo);
            }
            return listOfStudentsInfos;

        }

        // GET: api/Accounts/frank.miller
        [HttpGet("accountinfo")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<StudentsInfoM>> GetAccount(string username)
        {
            var sapUser = await _context.SAPs.FirstOrDefaultAsync(u => u.UserName == username);

            if (sapUser == null)
            {
                // Retourner une réponse 404 si l'utilisateur n'est pas trouvé
                return NotFound($"User with username {username} not found.");
            }
            StudentsInfoM studentInfo = await _studentService.GetUsersInfo(sapUser.UUID);
            return studentInfo;
        }

        [HttpGet("accountfromclass")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<IEnumerable<StudentsInfoM>>> GetUserByClass(string className)
        {
            var classEntity = await _context.Classes.FirstOrDefaultAsync(c => c.Name == className);
            var listOfStudents = await _context.SAPs
            .Where(s => s.ClassId == classEntity.ClassId)
            .Include(s => s.Class) // Inclure la classe associée
            .ToListAsync();
            List<StudentsInfoM> listStudentByClass = new List<StudentsInfoM>();
            foreach (var student in listOfStudents)
            {
                if (student.Class.Name == className)
                {
                    StudentsInfoM studentinfo = await _studentService.GetUsersInfo(student.UUID);
                    listStudentByClass.Add(studentinfo);
                }
            }
            return listStudentByClass;

        }

        [HttpPost("chargeamount")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<IActionResult> ChargeAccount(string username, double amount)
        {
            var account = await _studentService.GetAccountFromUsername(username);
            var uuid = User.FindFirst("UUID");
            var senderuuid = new Guid(uuid.Value);
            if (account == null)
            {
                return NotFound("This account doesn't exist");
            }
            await _transactionManagerService.AddCredit(account, amount);
            await _transactionManagerService.WriteTransaction(account.UUID, senderuuid, amount);
            return Ok("Transaction successful");

        }

        [HttpPost("chargeAll")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<IActionResult> ChargeAll(double amount)
        {
            var uuid = User.FindFirst("UUID");
            var senderuuid = new Guid(uuid.Value);
            var listStudent = await _context.SAPs.Where(ls => ls.Departement.DepartementName == "Students").ToListAsync();
            foreach (var student in listStudent)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UUID == student.UUID);
                await _transactionManagerService.AddCredit(account, amount);
                await _transactionManagerService.WriteTransaction(account.UUID, senderuuid, amount);
            }
          
            return Ok("Transaction successful");

        }

        [HttpGet("getUserTransaction")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<IEnumerable<TransactionM>>> GetTransaction(string username)
        {
            var account = await _studentService.GetAccountFromUsername(username);
            var transactions = await _studentService.GetTransactions(account.UUID);
            return Ok(transactions);

            {





            }
        }
    }
}
