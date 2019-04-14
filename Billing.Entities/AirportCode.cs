using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class AirportCode
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string Code { get; set; }
    }
}