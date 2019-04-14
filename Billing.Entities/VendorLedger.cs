using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Entities
{
    public class VendorLedger
    {
        [Key]
        [Column("Id", Order = 1)]
        public int Id { get; set; }
        public virtual VendorLedgerHead VendorLedgerHeads { get; set; }
        [Column("VendorLedgerHeadId", Order = 2)]
        public int VendorLedgerHeadId { get; set; }
        public virtual Vendor Vendors { get; set; }
        [Column("VendorId", Order = 3)]
        public virtual GeneralLedger GeneralLedgers { get; set; }
        [Column("GeneralLedgerId", Order = 4)]
        public int? GeneralLedgerId { get; set; }
        public virtual BankAccountLedger BankAccountLedgers { get; set; }
        [Column("BankAccountLedgerId", Order = 5)]
        public int? BankAccountLedgerId { get; set; }
        [Column("VendorId", Order = 6)]
        public int VendorId { get; set; }
        [Column("PaymentMethods", Order = 7)]
        public PaymentMethod? PaymentMethods { get; set; }
        [Column("Amount", Order = 8)]
        public double Amount { get; set; }
        [Column("Balance", Order = 9)]
        public double Balance { get; set; }
        [Column("Remarks", Order = 10)]
        public string Remarks { get; set; }
        [Column("SystemDate", Order = 11)]
        public DateTime SystemDate { get; set; }
        public virtual ApplicationUser ApplicationUsers { get; set; }
        [Column("ApplicationUserId", Order = 12)]
        public string ApplicationUserId { get; set; }
    }
}