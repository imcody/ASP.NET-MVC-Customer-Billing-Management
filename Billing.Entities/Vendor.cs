using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class Vendor
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string FaxNo { get; set; }
        public double NetSafi { get; set; }
        public double Atol { get; set; }
        public double Balance { get; set; }
        public DateTime AddedOn { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public string ApplicationUserId { get; set; }
    }
}
