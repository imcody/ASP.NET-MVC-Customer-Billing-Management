using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }
        public ProfileType InvoiceType { get; set; }
        public virtual Agent Agents { get; set; }
        public int AgentId { get; set; }
        public string GdsBookingDate { get; set; }
        public virtual Airlines AirlinesS { get; set; }
        public int AirlinesId { get; set; }
        public DateTime SysCreateDate { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
        public virtual Vendor Vendors { get; set; }
        public int VendorId { get; set; }
        public string VendorInvId { get; set; }
        public string Pnr { get; set; }
        public DateTime? ExpectedDatePayment { get; set; }
        public GDS GDSs { get; set; }
        public string GDSUserId { get; set; }
        public string CancellationChargeBefore { get; set; }
        public string CancellationChargeAfter { get; set; }
        public string CancellationDateBefore { get; set; }
        public string CancellationDateAfter { get; set; }
        public string NoShowBefore { get; set; }
        public string NoShowAfter { get; set; }
        public InvoiceStatus? InvoiceStatusS { get; set; }
        public InvoicePaymentStatus? PaymentStatus { get; set; }
        public double ExtraCharge { get; set; }
        public bool? PaidByAgent { get; set; }
        public bool? PaidToVendor { get; set; }
    }
}