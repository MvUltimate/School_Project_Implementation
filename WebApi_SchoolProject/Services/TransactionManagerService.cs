using DAL;
using DAL.Models;
using WebApi_SchoolProject.Models;

namespace WebApi_SchoolProject.Services
{
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

        public void AddCredit(Account account , double amount)
        {
            account.Amount += amount;
            _schoolContext.SaveChangesAsync();
        }

        public void  WriteTransaction(Guid accountCredited, Guid accountSender, double amount)
        {
            //Possible to do it with the table Account but we need to do one more link
            var credited =  _schoolContext.Accounts.FirstOrDefault(c=>c.UUID == accountCredited);
            var sender = _schoolContext.Accounts.FirstOrDefault(s => s.UUID == accountSender);
            var transaction = new Transaction
            {
                Receiver = credited.UUID,
                Sender = sender.UUID,
                Amount = amount,
                DateOnly = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                
            };
            _schoolContext.Transactions.Add(transaction);
            _schoolContext.SaveChanges();
            
        }

        

        
    }
}
