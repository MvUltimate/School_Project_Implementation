using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel;
using WebApi_SchoolProject.Models;

namespace WebApi_SchoolProject.Services
{
    //Service to carry out differents informations of a student
    public class StudentService
    {
        private readonly SchoolContext _context;
        private readonly TransactionManagerService _transactionManagerService;

        public StudentService(SchoolContext context, TransactionManagerService transactionManagerService)
        {
            _context = context;
            _transactionManagerService = transactionManagerService;
        }

        //Return the Account in function of the username in the SAPS Account
        // Will be used to compare the informations in the token
        //Used in StudentController
        public async Task<Account> GetAccountFromUsername(string username)
        {
           
            var user = await _context.SAPs.FirstOrDefaultAsync(u => u.UserName == username);
            if(user == null)
            {
                return null;
            }
            //Compare the UUID (Unique identifier) of the user with the UUID of all Account 
            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UUID == user.UUID);
            return account;

        }

        //Mapp the Model (studentInfoM)
        //Used to return all the informations that the student need 
        public async Task<StudentsInfoM> GetUsersInfo(Guid uuid)
        {

            // Retrieve informations of the user (SAP)
            var sapUser = await _context.SAPs
                .Include(s => s.Class) // Include the studentClass
                .FirstOrDefaultAsync(u => u.UUID == uuid);

            // Retrieve the user Account 
            var userAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UUID == sapUser.UUID);

            // Mapp the informations to the StudentsInfoM
            var userInfo = new StudentsInfoM
            {
                Uuid = sapUser.UUID,
                UserName = sapUser.UserName,
                Amount = userAccount.Amount,
                NbrPage = _transactionManagerService.ConvertMoneyToQuota(userAccount.Amount),
                Class = sapUser.Class?.Name,
                DepartmentId = sapUser.DepartementId,
            };

            return userInfo;
        }

        //Get the list of transaction from a unique identifier (UUID)
        //Is used with the UUID because it will be called uniquely by the student authentified and
        //the UUID is stored in the Bearer token
        public async Task<List<TransactionM>> GetTransactions(Guid uuid)
        {
            var transactions = await _context.Transactions.Where(t => t.Receiver == uuid).ToListAsync();
            
            List<TransactionM> transactionMs = new List<TransactionM>();
            foreach(var transaction in transactions)
            {
                var sender = await _context.SAPs.FirstOrDefaultAsync(s => s.UUID == transaction.Sender);
                var receiver = await _context.SAPs.FirstOrDefaultAsync(r=> r.UUID == transaction.Receiver);
                var transactionM = new TransactionM
                {   
                    TransactionId = transaction.TransactionId,
                    Receiver = receiver.UserName,
                    Sender = sender.UserName,
                    Amount = transaction.Amount,
                    Date = transaction.DateOnly
                };
                transactionMs.Add(transactionM);
            }
            return transactionMs;
        }

        public async Task<AmountPagesM> GetNumberOfPage(string username)
        {
            // Retrieve informations of the user (SAP)
            var sapUser = await _context.SAPs
                .Include(s => s.Class) // Include the studentClass
                .FirstOrDefaultAsync(u => u.UserName == username);

            // Retrieve the user Account 
            var userAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UUID == sapUser.UUID);

            // Mapp the informations to the StudentsInfoM
            var userInfo = new AmountPagesM
            {
                UserName = sapUser.UserName,
                NbrPage = _transactionManagerService.ConvertMoneyToQuota(userAccount.Amount),
            };

            return userInfo;
        }


    }
}
