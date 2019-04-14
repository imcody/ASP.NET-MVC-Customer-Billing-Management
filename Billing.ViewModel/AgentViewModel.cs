using Billing.Entities;

namespace Billing.ViewModel
{
    public class AgentOutstandingInvoiceListViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }

    }
    public class AgentBulkInvoicePaymentViewModel
    {
        public virtual Agent Agents { get; set; }
        public int AgentId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string PaymentDate { get; set; }

    }
    public class InvoicePaymentHistory
    {
        public int InvoiceId { get; set; }
        public string PaymentDate { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string PaymentMethod { get; set; }
    }
    public class AgentLedgerViewModel
    {
        public int LedgerId { get; set; }
        public string LedgerDate { get; set; }
        public string LedgerHead { get; set; }
        public string Notes { get; set; }
        public string LedgerType { get; set; }
        public double Amount { get; set; }
        public double Credit { get; set; }
        public double Debit { get; set; }
        public double Balance { get; set; }
    }
}