using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class OtherInvoice
    {
        [Key]
        public int Id { get; set; }
        public virtual Agent Agents { get; set; }
        [Required]
        public int AgentId { get; set; }
        public virtual Vendor Vendors { get; set; }
        [Required]
        public int VendorId { get; set; }
        public virtual OtherInvoiceType OtherInvoiceTypes { get; set; }
        [Required]
        public int OtherInvoiceTypeId { get; set; }
        public string Reference { get; set; }
        public string ExpectedPayDate { get; set; }
        public string VendorInvNo { get; set; }
        public string Details { get; set; }
        [Required]
        public double CustomerAgentAmount { get; set; }
        [Required]
        public double VendorAmount { get; set; }
        public bool CustomerAgentPaid { get; set; }
        public bool VendorPaid { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}