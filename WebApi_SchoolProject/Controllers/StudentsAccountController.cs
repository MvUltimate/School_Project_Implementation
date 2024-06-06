using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Transactions;
using WebApi_SchoolProject.Models;
using WebApi_SchoolProject.Services;

namespace WebApi_SchoolProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsAccountController : ControllerBase
    {

        private readonly StudentService _studentService;
        private readonly TransactionManagerService _transactionManagerService;

        public StudentsAccountController(StudentService studentService, TransactionManagerService transactionManagerService)
        {
            _studentService = studentService;
            _transactionManagerService = transactionManagerService;
        }

        //Get the information in function de who is connected
        [HttpGet("infos")]
        public async Task<IActionResult>GetUserInfo()
        {
            //will grab the UUID in the Token Claims to return the informations in function
            var uuidClaim = User.FindFirst("UUID");
            if (uuidClaim == null)
            {
                return Unauthorized("User not authenticated");
            }
            var userName = new Guid(uuidClaim.Value);

            var userInfo = await _studentService.GetUsersInfo(userName);
            return Ok(userInfo);
        }

        //Charge the account of the student connected
        [HttpPost("chargeAccount")]
        public async Task<IActionResult> ChargeAccount([FromBody]ChargeRequest chargequest)
        {
            var userNameClaim =  User.FindFirst(ClaimTypes.Name);
            if (userNameClaim == null)
            {
                return Unauthorized("User not authenticated");
            }
            var account = await _studentService.GetAccountFromUsername(userNameClaim.Value);
            await _transactionManagerService.AddCredit(account, chargequest.amount);
            //The transaction is done by the Student Himself
            await _transactionManagerService.WriteTransaction(account.UUID, account.UUID, chargequest.amount);
            return Ok("Account charged successfully");
        }

        public class ChargeRequest()
        {
            public double amount { get; set; }
        } 

        //Get the transactions of the student connected 
        [HttpGet("transactions")]
        public async Task<ActionResult<IEnumerable<TransactionM>>> GetTransactions()
        {
            var uuidClaim = User.FindFirst("UUID");
            if (uuidClaim == null)
            {
                return Unauthorized("User not authenticated");
            }
            var uuid = new Guid(uuidClaim.Value);
            var transactions = await _studentService.GetTransactions(uuid);
            return Ok(transactions);
        }

        

        [HttpGet("infos/{username}")]
        public async Task<IActionResult> GetUserInfo(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return BadRequest("Username is required");
            }

            var userInfo = await _studentService.GetNumberOfPage(username);
            if (userInfo == null)
            {
                return NotFound("User not found");
            }

            return Ok(userInfo);
        }
        


    }
}
