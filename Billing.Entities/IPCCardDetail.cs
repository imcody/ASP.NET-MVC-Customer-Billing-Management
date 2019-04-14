using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class IPCCardDetail
    {
        [Key]
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int? InvoiceId { get; set; }
        public virtual OtherInvoice OtherInvoices { get; set; }
        public int? OtherInvoiceId { get; set; }
        public virtual InvoicePayment InvoicePayments { get; set; }
        public int? InvoicePaymentId { get; set; }
        public virtual BankAccount BankAccounts { get; set; }
        public int BankAccountId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}