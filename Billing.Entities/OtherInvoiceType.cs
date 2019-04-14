using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class OtherInvoiceType
    {
        [Key]
        public int Id { get; set; }
        public string InvoiceType { get; set; }
        public DateTime CreatedOn { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}