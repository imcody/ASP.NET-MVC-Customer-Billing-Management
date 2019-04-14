using System;

namespace Billing.Entities
{
    public class OtherInvoicePaymentVendor
    {
        public int Id { get; set; }
        public virtual OtherInvoice OtherInvoices { get; set; }
        public int OtherInvoiceId { get; set; }
        public virtual Vendor Vendors { get; set; }
        public int VendorId { get; set; }
        public PaymentMethod PaymentMethods { get; set; }
        public virtual GeneralLedger GeneralLedger { get; set; }
        public int? GeneralLedgerId { get; set; }
        public virtual VendorLedger VendorLedgers { get; set; }
        public int VendorLedgerId { get; set; }
        public virtual BankAccountLedger BankAccountLedgers { get; set; }
        public int? BankAccountLedgerId { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public DateTime SysDateTime { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}