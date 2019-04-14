using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class GeneralLedgerHead
    {
        [Key]
        public int Id { get; set; }
        public string GeneralLedgerHeadName { get; set; }
        public LedgerType GeneralLedgerType { get; set; }
        public bool Editable { get; set; }
        public bool Status { get; set; }
    }
}
