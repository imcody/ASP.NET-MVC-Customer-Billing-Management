using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class IPChequeDetail
    {
        //Cheque Payment for independent Invoice..
        [Key]
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int? InvoiceId { get; set; }
        public virtual OtherInvoice OtherInvoices { get; set; }
        public int? OtherInvoiceId { get; set; }
        public virtual InvoicePayment InvoicePayments { get; set; }
        public int? InvoicePaymentId { get; set; }
        public virtual GeneralLedger GeneralLedger { get; set; }
        public int? GeneralLedgerId { get; set; }
        public BankName BankNames { get; set; }
        public string AccountNo { get; set; }
        public string ChequeNo { get; set; }
        public string SortCode { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime SysCreateDate { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
        public ChequeStatus Status { get; set; }
        public bool? BulkPayment { get; set; }
        public int? ProfileType { get; set; }   //1 = Agent, 2 = Vendor, 3 = Customer
    }
}