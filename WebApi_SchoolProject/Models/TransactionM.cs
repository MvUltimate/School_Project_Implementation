using DAL.Models;

namespace WebApi_SchoolProject.Models
{
    public class TransactionM
    {

        public int TransactionId { get; set; }
        public string Receiver { get; set; } // Compte sur lequel est effectuée la transaction

        public string Sender { get; set; }
        public string Date { get; set; }
        public double Amount { get; set; }

        

    }
}
