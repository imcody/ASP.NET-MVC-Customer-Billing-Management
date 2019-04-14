using System;

namespace Billing.Entities
{
    public class InvoiceLog
    {
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int InvoiceId { get; set; }
        public string Remarks { get; set; }
        public virtual ApplicationUser ApplicationUers { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime SysDateTime { get; set; }
    }
}