using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Entities
{
    public class VendorLedgerHead
    {
        [Key]
        public int Id { get; set; }
        [StringLength(255)]
        [Index(IsUnique = true)]
        public string LedgerHead { get; set; }
        public LedgerType LedgerTypes { get; set; }
        public bool Edit { get; set; }
        public bool Status { get; set; }
    }
}
