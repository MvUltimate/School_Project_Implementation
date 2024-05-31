using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Transaction
    {

        public int TransactionId { get; set; }
        public Guid Receiver { get; set; } //Compte sur lequel est effectué la transaction
        public string DateOnly { get; set; }
        public double Amount { get; set; } 

        public Guid Sender { get; set; }

    }
}
