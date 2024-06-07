using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace DAL.Models
{
    public class Account
    {

        public int AccountID { get; set; }
        public Guid UUID {  get; set; }

        public double Amount { get; set; } = 0.0;

        public ICollection<Transaction> Transactions { get;} = new List<Transaction>();
        
    }
}
