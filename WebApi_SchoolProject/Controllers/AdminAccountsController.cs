using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL;
using WebApi_SchoolProject.Services;
using WebApi_SchoolProject.Models;
using Microsoft.AspNetCore.Authorization;


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

        //...api/adminAccount/createaccount/uuid
        // create an Account in the table Account based on a UUID in the table SAP 
        [HttpPost("createaccount {Uuid}")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public IActionResult CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                _accountService.CreateUser(request.UUID, request.Password);
                return Ok("User created successfully ");
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

        //...api/adminaccount/chargeClass/601-PT
        [HttpPost("chargeClass")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task <ActionResult> ChargeClass([FromBody] ChargeClassRequest chargeClassRequest)
        {
            //retrieve the UUID of the authentified user to store it in the transaction
            var uuid = User.FindFirst("UUID");
            var senderuuid = new Guid(uuid.Value);

            //retrieve the class
            //his ID is used to get the list of students
            var studentClass = await _context.Classes.FirstOrDefaultAsync(c => c.Name == chargeClassRequest.Class);
            if (studentClass == null)
            {
                return NotFound($"Class with name {studentClass} not found.");
            }

            var listOfStudents = await _context.SAPs
            .Where(s => s.ClassId == studentClass.ClassId)
            .Include(s => s.Class) // Inclure la classe associée
            .ToListAsync();

            //Foreach student we retrieve his account and charge it with the amount and store a new transaction
            foreach (var student in listOfStudents)
            {
                if (student.Class.Name == studentClass.Name)
                {
                    var account = await _studentService.GetAccountFromUsername(student.UserName);
                    //var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UUID == student.UUID);
                    await _transactionManagerService.AddCredit(account, chargeClassRequest.Amount);
                    await _transactionManagerService.WriteTransaction(account.UUID, senderuuid, chargeClassRequest.Amount);
                }
            }

            return Ok("Transactions successfull");

        }

        public class ChargeClassRequest
        {
            public double Amount { get; set; }
            public string Class { get; set; }
        }

        // GET: api/adminaccount
        //return all the accounts
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

        // GET: api/adminaccount/accountinfo/frank.miller
        //return the information (StudentsInfoM) from a specific user
        [HttpGet("accountinfo")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<StudentsInfoM>> GetAccount(string username)
        {
            var sapUser = await _context.SAPs.FirstOrDefaultAsync(u => u.UserName == username);

            if (sapUser == null)
            {
                return NotFound($"User with username {username} not found.");
            }
            StudentsInfoM studentInfo = await _studentService.GetUsersInfo(sapUser.UUID);
            return studentInfo;
        }


        //...api/adminaccount/accountfromclass
        // retrieve the user in function of a className
        [HttpGet("accountfromclass")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<IEnumerable<StudentsInfoM>>> GetUserByClass(string className)
        {

            var studentClass = await _context.Classes.FirstOrDefaultAsync(c => c.Name == className);

            var listOfStudents = await _context.SAPs
            .Where(s => s.ClassId == studentClass.ClassId)
            .Include(s => s.Class) // Inclure studenClass
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

        //...api/cadminaccount/chargeamount/frank.miller&10
        // charge amount of a specific user by his username
        [HttpPost("chargeamount")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<IActionResult> ChargeAccount(string username, double amount)
        {
            var account = await _studentService.GetAccountFromUsername(username);
            if (account == null)
            {
                return NotFound($"User with username {username} not found.");
            }

            var uuid = User.FindFirst("UUID");
            var senderuuid = new Guid(uuid.Value);

            await _transactionManagerService.AddCredit(account, amount);
            await _transactionManagerService.WriteTransaction(account.UUID, senderuuid, amount);
            return Ok("Transaction successful");

        }

        // ...api/adminaccounts/chargeall
        // charge the account of all students
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
        //return the transaction for a specifique user
        [HttpGet("getUserTransaction")]
        [Authorize(Policy = "RequireAdminDepartement")]
        public async Task<ActionResult<IEnumerable<TransactionM>>> GetTransaction(string username)
        {
            var account = await _studentService.GetAccountFromUsername(username);

            var transactions = await _studentService.GetTransactions(account.UUID);

            return Ok(transactions);
        }
    }
}
