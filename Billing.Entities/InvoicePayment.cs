using System;

namespace Billing.Entities
{
    public class InvoicePayment
    {
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int InvoiceId { get; set; }
        public virtual Agent Agents { get; set; }
        public int AgentId { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public virtual GeneralLedger GeneralLedger { get; set; }
        public int GeneralLedgerId { get; set; }
        public virtual AgentLedger AgentLedgers { get; set; }
        public int AgentLedgerId { get; set; }
        public virtual BankAccountLedger BankAccountLedgers { get; set; }
        public int? BankAccountLedgerId { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime SysDateTime { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}