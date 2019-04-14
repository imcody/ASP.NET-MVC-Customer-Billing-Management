using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.DAL.Parameters
{
    public class AgentBulkPaymentCashVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public string LedgerDate { get; set; }
    }
    public class AgentBulkPaymentChequeVoucher
    {
        public int AgentId { get; set; }
        public int BankNames { get; set; }
        public string AccountNo { get; set; }
        public string ChequeNo { get; set; }
        public string SortCode { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public bool BulkPayment { get; set; }
    }
    public class BulkPaymentAgentCashInvPaymentInvLog
    {
        public int InvoiceId { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public int GeneralLedgerId { get; set; }
        public int AgentLedgerId { get; set; }
        public int BankAccountLedgerId { get; set; }
        public int AgentId { get; set; }
        public string LedgerDate { get; set; }
    }
    public class AgentBulkPaymentCreditCard
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public int BankAccountId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
    }
    public class AgentBulkPaymentDebitCard
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public int BankAccountId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
    }
    public class AgentBulkPaymentBankDeposit
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public int BankAccountId { get; set; }
        public string BankDate { get; set; }
    }
    public class AgentBulkPaymentChequePayment
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public int BankAccountId { get; set; }
        public string BankDate { get; set; }
        public int ChekId { get; set; }
    }
}