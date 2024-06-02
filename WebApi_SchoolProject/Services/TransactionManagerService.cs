using DAL;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using WebApi_SchoolProject.Models;

namespace WebApi_SchoolProject.Services
{
    //Service to manage the different transactions
    public class TransactionManagerService
    {

        private readonly SchoolContext _schoolContext;
        public TransactionManagerService(SchoolContext context)
        {
            _schoolContext = context;
        }

        public int ConvertMoneyToQuota(double amount)
        {
            return (int)(amount / 0.05);
        }

       
        public async Task AddCredit(Account account , double amount)
        {
            account.Amount += amount;

            await _schoolContext.SaveChangesAsync();
        }

       
        public async Task  WriteTransaction(Guid accountCredited, Guid accountSender, double amount)
        {
            
            var credited = await _schoolContext.Accounts.FirstOrDefaultAsync(c => c.UUID == accountCredited);
            var sender = await _schoolContext.Accounts.FirstOrDefaultAsync(s => s.UUID == accountSender);
            var transaction = new Transaction
            {
                Receiver =  credited.UUID,
                Sender = sender.UUID,
                Amount = amount,
                DateOnly = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                
            };
            _schoolContext.Transactions.Add(transaction);
            await _schoolContext.SaveChangesAsync();
            
        } 

        
    }
}
