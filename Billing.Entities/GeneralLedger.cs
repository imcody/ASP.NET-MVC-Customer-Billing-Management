using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class GeneralLedger
    {
        [Key]
        public int Id { get; set; }
        public TransactionType StatementTypes { get; set; } //Defines Whether to put in Income Statement or Expense Statement
        public virtual GeneralLedgerHead GeneralLedgerHeads { get; set; }
        public int GeneralLedgerHeadId { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public virtual LedgerType GeneralLedgerType { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
        public DateTime SysDateTime { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
