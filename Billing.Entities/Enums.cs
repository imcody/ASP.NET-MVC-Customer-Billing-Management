using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public enum LedgerType
    {
        [Display(Name = "Debit")]
        Debit = 1,
        [Display(Name = "Credit")]
        Credit = 2
    }
    public enum UserRole
    {
        Administrator = 1,
        [Display(Name = "Call Center Agent")]
        Agent = 2,
        [Display(Name = "Accounts Manager")]
        Accounts = 3
    }
    public enum MaritialStatus
    {
        Single = 1,
        Married = 2,
        Divorced = 3
    }
    public enum Sex
    {
        Male = 1,
        Female = 2
    }
    public enum UserStatus
    {
        Active = 1,
        Locked = 2
    }
    public enum ProfileType
    {
        Agent = 1,
        Customer = 2
    }
    public enum InvoiceStatus
    {
        Draft = 1,
        Raised = 2,
        Void = 3,
        Refund = 4
    }
    public enum InvoicePaymentStatus
    {
        Due = 1,
        Paid = 2,
        PartialPaid = 3
    }
    public enum GDS
    {
        Amadeus = 1,
        Galileo = 2,
        Sabre = 3,
        [Display(Name = "Fare Logix")]
        FareLogix = 4
    }
    public enum PassengerType
    {
        ADT = 1,
        CHD = 2,
        SRC = 3,
        YTH = 4,
        INF = 5
    }
    public enum PaymentMethod
    {
        Cash = 1,
        Cheque = 2,
        [Display(Name = "Credit Card")]
        CreditCard = 3,
        [Display(Name = "Debit Card")]
        DebitCard = 4,
        [Display(Name = "Bank Deposit")]
        BankDeposit = 5,
        [Display(Name = "Invoice Adjust")]
        InvoiceAdjust = 6,
        [Display(Name = "Inter Transaction")]
        InterTransaction = 7,
        [Display(Name = "BDT amount received")]
        BDTAmountReceived = 8
    }
    public enum BankDepositPaymentMethod
    {
        Cash = 1,
        Cheque = 2,
        [Display(Name = "Credit Card")]
        CreditCard = 3,
        [Display(Name = "Debit Card")]
        DebitCard = 4,
        [Display(Name = "Bank Deposit")]
        BankDeposit = 5
    }
    public enum BankName
    {
        [Display(Name = "Royal Bank of Scotland")]
        CityBankLimited = 1,
        [Display(Name = "NatWest")]
        JanataBankLtd = 2,
        [Display(Name = "HSBC Bank")]
        HSBC = 3,
        [Display(Name = "Barclays Bank")]
        Barclays = 4,
        [Display(Name = "Lloyds Bank")]
        Lloyds = 5
    }
    public enum TransactionType
    {
        Income = 1,
        Expense = 2
    }
    public enum ChequeStatus
    {
        Floating = 1,
        Passed = 2,
        Withdrawn = 3
    }
}
