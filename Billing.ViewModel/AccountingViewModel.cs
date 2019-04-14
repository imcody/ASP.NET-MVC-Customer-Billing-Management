using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Billing.Entities;

namespace Billing.ViewModel
{
    public class ChequeRealizeViewModel
    {
        [Key]
        public int InvoiceId { get; set; }
        public int IPChequeDetailId { get; set; }
        public string BankName { get; set; }
        public string ChequeNo { get; set; }
        public string AccountNo { get; set; }
        public double Amount { get; set; }
        public string SortCode { get; set; }
        public string Remarks { get; set; }
        public BankName BankNames { get; set; }
        public virtual BankAccount BankAccounts { get; set; }
        public int BankAccountId { get; set; }
        public virtual Agent Agents { get; set; }
        public string RealizationRemarks { get; set; }
    }
    public class FloatChequeViewModel
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public string IssueDate { get; set; }
        public BankName BankName { get; set; }
        public string CheuqueNo { get; set; }
        public string AccountNo { get; set; }
        public string SortCode { get; set; }
        public string Remarks{ get; set; }
        public double Amount { get; set; }
        public int ProfileType { get; set; }
    }
    public class GeneralVoucherPostingViewModel
    {
        public virtual List<GeneralLedgerHead> GeneralLedgerHeads { get; set; }
        public int GeneralLedgerHeadId { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
    }
    public class GeneralLedgerListViewModel
    {
        public string LedgerDate { get; set; }
        public string UserName { get; set; }
        public string LedgerHead { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
        public string LedgerType { get; set; }
        public double Amount { get; set; }
    }
}