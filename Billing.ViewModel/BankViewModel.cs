using Billing.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Billing.ViewModel
{
    public class AddBankViewModel
    {
        [Required]
        [Display(Name = "Select Bank")]
        public BankName BankNames { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [StringLength(40, ErrorMessage = "The {0} must be at least 8 characters long.", MinimumLength = 8)]
        public string AccountNo { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        [StringLength(40, ErrorMessage = "The {0} must be at least 8 characters long.", MinimumLength = 12)]
        public string AccountNames { get; set; }

        [Required]
        [Display(Name = "Opening Balance")]
        public double Balance { get; set; }
    }
    public class BankAccountListViewModel
    {
        [Display(Name = "Bank")]
        public BankName BankNames { get; set; }

        [Display(Name = "A/C No")]
        public string AccountNo { get; set; }

        [Display(Name = "A/C Name")]
        public string AccountNames { get; set; }

        [Display(Name = "Balance")]
        public double Balance { get; set; }
    }
    public class BankAccountLedgerViewModel
    {
        public int LedgerId { get; set; }
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        public double Amount { get; set; }
        [Display(Name = "Date")]
        public DateTime VoucherDate { get; set; }
        [Display(Name = "Particulars")]
        public string Particulars { get; set; }
        [Display(Name = "Particulars")]
        public string Notes { get; set; }
        [Display(Name = "Vch Type")]
        public LedgerType LedgerTypes { get; set; }
        [Display(Name = "Balance")]
        public double Balance { get; set; }
        public int MyProperty { get; set; }
    }
    public class BankDepositViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Bank")]
        public BankName BankNames { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name ="A/C No")]
        public int BankAccountId { get; set; }
        public PaymentMethod TransactionMethod { get; set; }
        public virtual List<BankAccountLedgerHead> BankAccountLedgerHeads { get; set; }
        public int BankAccountLedgerHeadId { get; set; }
        [Display(Name = "Date")]
        [Required]
        public string LedgerDate { get; set; }
        [Display(Name = "Amount")]
        [Required]        
        public double Amount { get; set; }
        [Display(Name = "Notes")]
        public string Notes { get; set; }
        #region Bank Deposit - Payment Method * Cheque
        public int? BankOfChequeId { get; set; }
        public string ChequeNo { get; set; }
        public string AccountNo { get; set; }
        public string SortCode { get; set; }
        #endregion
        #region Bank Deposit - Payment Method * Credit Card
        public string CreditCardNo { get; set; }
        public string CardHolderName { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; } 
        #endregion
    }
    public class BankWithdrawalViewModel
    {
        [Required]
        public BankName BankNames { get; set; }
        [Required]
        public int BankAccountId { get; set; }
        public virtual List<BankAccountLedgerHead> BankAccountLedgerHeads { get; set; }
        [Required]
        public int BankAccountLedgerHeadId { get; set; }
        [Required]
        public string ChequeNo { get; set; }
        [Required]
        public double Amount { get; set; }
        public string Notes { get; set; }
        [Required]
        public string LedgerDate { get; set; }
        public string UserID { get; set; }
    }
}