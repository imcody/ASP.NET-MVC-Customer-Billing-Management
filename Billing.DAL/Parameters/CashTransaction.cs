namespace Billing.DAL.Parameters
{
    public class CashTransaction
    {
        public double Amount { get; set; }
        public string Remarks { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public int InvoiceId { get; set; }
    }
}