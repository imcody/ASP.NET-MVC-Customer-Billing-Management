using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.DAL.Parameters
{
    public class VendorBulkPaymentCashVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public int VendorId { get; set; }
        public string LedgerDate { get; set; }
    }
    public class VendorBulkPaymentChequeVoucher
    {
        public int VendorId { get; set; }
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
    public class VendorBulkPaymentCreditCard
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int VendorId { get; set; }
        public int BankAccountId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
    }
    public class VendorBulkPaymentDebitCard
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int VendorId { get; set; }
        public int BankAccountId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
    }
    public class VendorBulkPaymentBankDeposit
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int VendorId { get; set; }
        public int BankAccountId { get; set; }
        public string BankDate { get; set; }
    }
    public class BulkPaymentVendorCashInvPaymentInvLog
    {
        public int InvoiceId { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public int GeneralLedgerId { get; set; }
        public int VendorLedgerId { get; set; }
        public int BankAccountLedgerId { get; set; }
        public int VendorId { get; set; }
        public string LedgerDate { get; set; }
    }
    public class VendorBulkPaymentChequePayment
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public string UserId { get; set; }
        public int VendorId { get; set; }
        public int BankAccountId { get; set; }
        public string BankDate { get; set; }
        public int ChekId { get; set; }
    }
}