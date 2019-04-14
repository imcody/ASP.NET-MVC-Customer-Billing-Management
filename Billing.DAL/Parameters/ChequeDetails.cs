using Billing.Entities;

namespace Billing.DAL.Parameters
{
    public class ChequeDetails
    {
        public int InvoiceId { get; set; }
        public BankName BankNames { get; set; }
        public string AccountNo { get; set; }
        public string ChequeNo { get; set; }
        public string SortCode { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
    }
}