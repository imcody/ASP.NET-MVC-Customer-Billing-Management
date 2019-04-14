using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Billing.Entities
{
    public class AgentLedger
    {
        [Key]
        [Column("Id", Order = 1)]
        public int Id { get; set; }
        public virtual AgentLedgerHead AgentLedgerHeads { get; set; }
        [Column("AgentLedgerHeadId", Order = 2)]
        public int AgentLedgerHeadId { get; set; }
        public virtual Agent Agents { get; set; }
        [Column("AgentId", Order = 3)]
        public int AgentId { get; set; }
        public virtual GeneralLedger GeneralLedgers { get; set; }
        [Column("GeneralLedgerId", Order = 4)]
        public int? GeneralLedgerId { get; set; }
        [Column("PaymentMethods", Order = 6)]
        public PaymentMethod? PaymentMethods { get; set; }
        [Column("PaymentType", Order = 7)]
        public int? PaymentType { get; set; }
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
        public virtual BankAccountLedger BankAccountLedgers { get; set; }
        [Column("BankAccountLedgerId", Order = 5)]
        public int? BankAccountLedgerId { get; set; }
    }
}