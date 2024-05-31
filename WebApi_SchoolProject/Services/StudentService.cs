using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.ComponentModel;
using WebApi_SchoolProject.Models;

namespace WebApi_SchoolProject.Services
{
    //Class who return informations from differents tables to have all the informations in one view
    public class StudentService
    {
        private readonly SchoolContext _context;
        private readonly TransactionManagerService _transactionManagerService;

        public StudentService(SchoolContext context, TransactionManagerService transactionManagerService)
        {
            _context = context;
            _transactionManagerService = transactionManagerService;
        }

        //Return the account in function of the username
        // Will be used to compare the informations in the token
        //Used in StudentController TO Charge Amount
        public async Task<Account> GetAccountFromUsername(string username)
        {
            using var context = new SchoolContext();
            var user = await _context.SAPs.FirstOrDefaultAsync(u => u.UserName == username);
            if(user == null)
            {
                return null;
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.UUID == user.UUID);
            return account;

        }

        //Mapper la classe studentInfo
        //Used to return all the informations of a student or the list of student for the Admin
        public async Task<StudentsInfoM> GetUsersInfo(Guid uuid)
        {

            // Récupérer les informations de l'utilisateur (SAP)
            var sapUser = await _context.SAPs
                .Include(s => s.Class) // Inclure la classe associée
                .FirstOrDefaultAsync(u => u.UUID == uuid);

            // Récupérer le compte de l'utilisateur
            var userAccount = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UUID == sapUser.UUID);

            // Mapper les informations dans le DTO StudentInfo
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

                    Receiver = receiver.UserName,
                    Sender = sender.UserName,
                    Amount = transaction.Amount,
                    Date = transaction.DateOnly
                };
                transactionMs.Add(transactionM);
            }
            return transactionMs;
        }


    }
}
