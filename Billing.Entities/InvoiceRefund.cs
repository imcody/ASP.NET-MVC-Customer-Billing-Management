using System;

namespace Billing.Entities
{
    public class InvoiceRefund
    {
        public int Id { get; set; }
        public DateTime RefundDate { get; set; }
        public virtual Vendor Vendors { get; set; }
        public int VendorId { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int InvoiceId { get; set; }
        public string ProfileName { get; set; }
        public string TicketNo { get; set; }
        public double CustomerFare { get; set; }
        public double InvoiceTax { get; set; }
        public double InvoiceNetFare { get; set; }
        public double UserFare { get; set; }
        public double CancellationFee { get; set; }
        public double RefundTax { get; set; }
        public double CustomerCancellationFee { get; set; }
        public double VendorCharge { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}