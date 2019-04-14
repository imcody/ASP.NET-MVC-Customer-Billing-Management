using System;

namespace Billing.Entities
{
    public class OtherInvoiceLog
    {
        public int Id { get; set; }
        public virtual OtherInvoice OtherInvoices { get; set; }
        public int OtherInvoiceId { get; set; }
        public string Remarks { get; set; }
        public virtual ApplicationUser ApplicationUers { get; set; }
        public string ApplicationUserId { get; set; }
        public DateTime SysDateTime { get; set; }
    }
}