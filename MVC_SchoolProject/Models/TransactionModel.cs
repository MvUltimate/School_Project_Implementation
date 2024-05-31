using System.ComponentModel.DataAnnotations;

namespace MVC_SchoolProject.Models
{
    public class TransactionModel
    {
        public int TransactionId { get; set; }

        [Display(Name = "Receiver Account")]
        public string Receiver { get; set; } // Compte recevant l'argent

        [Display(Name = "Sender Account")]
        public string Sender { get; set; } // Compte envoyant l'argent

        [Display(Name = "Transaction Date")]
        public string Date { get; set; } // Utilisation de DateTime pour une meilleure gestion des dates

        [Display(Name = "Amount")]
        public double Amount { get; set; } // Montant de la transaction
    }
}
