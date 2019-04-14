using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class CompanyInfo
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        [Display(Name = "Fax")]public string FaxNo { get; set; }
        public string Email { get; set; }
        [Display(Name = "Web Address")]
        public string WebAddress { get; set; }
        public double Atol { get; set; }
        [Display(Name = "Customer Safi")]
        public double CustomerSafi { get; set; }
        [Display(Name = "Credit Card Charge")]
        public double CreditCardCharge { get; set; }
        [Display(Name = "Debit Card Charge")]
        public double DebitCardCharge { get; set; }
        public double Apc { get; set; }
        [Display(Name = "Agent Safi")]
        public double AgentSafi { get; set; }
    }
}