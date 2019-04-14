using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Entities
{
    public class BankAccountLedgerHead
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        [Index("IX_FirstAndSecond", 1, IsUnique = true)]
        public string LedgerHead { get; set; }
        public LedgerType LedgerTypes { get; set; }
        public bool Editable { get; set; }
        public bool Status { get; set; }
        public virtual GeneralLedgerHead GeneralLedgerHeads { get; set; }
        public int? GeneralLedgerHeadId { get; set; }
    }
}