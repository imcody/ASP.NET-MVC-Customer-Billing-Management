using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        public BankName BankNames { get; set; }
        public string AccountNo { get; set; }
        public string AccountNames { get; set; }
        public double Balance { get; set; }
        public DateTime AddedOn { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
