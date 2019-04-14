using System;

namespace Billing.Entities
{
    public class BankAccountLedger
    {
        public int Id { get; set; }
        public virtual BankAccount BankAccounts { get; set; }
        public int BankAccountId { get; set; }
        public virtual BankAccountLedgerHead BankAccountLedgerHeads { get; set; }
        public int BankAccountLedgerHeadId { get; set; }
        public LedgerType LedgerTypes { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public virtual GeneralLedger GeneralLedgers { get; set; }
        public int? GeneralLedgerId { get; set; }
        public int? RelationId { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public double Balance { get; set; }
        public DateTime SysDateTime { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
