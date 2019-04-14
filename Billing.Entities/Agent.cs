using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class Agent
    {
        [Key]
        public int Id { get; set; }
        public ProfileType ProfileType { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string FaxNo { get; set; }
        public string Atol { get; set; }
        public double CreditLimit { get; set; }
        public double Balance { get; set; }
        public string Remarks { get; set; }
        public DateTime JoiningDate { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public string ApplicationUserId { get; set; }
    }
}