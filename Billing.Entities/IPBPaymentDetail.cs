using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class IPBPaymentDetail
    {
        [Key]
        public int Id { get; set; }
        public virtual BankAccountLedger BankAccountLedgers { get; set; }
        public int? BankAccountLedgerId { get; set; }
        public virtual BankAccount BankAccounts { get; set; }
        public int BankAccountId { get; set; }
        public string Remarks { get; set; }
        public string BankDate { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}