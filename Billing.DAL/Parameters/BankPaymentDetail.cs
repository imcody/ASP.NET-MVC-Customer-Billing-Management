using System.ComponentModel.DataAnnotations;

namespace Billing.DAL.Parameters
{
    public class BankPaymentDetail
    {
        [Key]
        public int Id { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public int InvoiceId { get; set; }
        public int BankAccountId { get; set; }
        public string BankDate { get; set; }
        public string UserId { get; set; }
    }
    public class BankDepositCashVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public int BankAccountId { get; set; }
        public string LedgerDate { get; set; }
        public int BankLedgerHeadId { get; set; }
    }
    public class BankDepositChequeVoucher
    {
        public double Amount { get; set; }
        public int BankAccountId { get; set; }
        public int BankOfChequeId { get; set; }
        public string ChequeNo { get; set; }
        public string AccountNo { get; set; }
        public string SortCode { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public string LedgerDate { get; set; }
        public int ChequeStauts { get; set; }
    }
    public class InsertNewBankAccount
    {
        public int BankNames { get; set; }
        public string AccountNo { get; set; }
        public string AccountNames { get; set; }
        public double Balance { get; set; }
        public string UserId { get; set; }
    }
    public class BankDepositCreditCardVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public int BankAccountId { get; set; }
        public string UserID { get; set; }
        public int LedgerHeadId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
    }
    public class BankDepositDebitCardVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string LedgerDate { get; set; }
        public int BankAccountId { get; set; }
        public string UserID { get; set; }
        public int LedgerHeadId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
    }
    public class BankDepositDirectDepositVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public int BankAccountId { get; set; }
        public string LedgerDate { get; set; }
        public int BankLedgerHeadId { get; set; }
    }
    public class BankWithdrawalChequeVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public int BankAccountId { get; set; }
        public int BankLedgerHeadId { get; set; }
        public int BankId { get; set; }
        public string LedgerDate { get; set; }
    }

}