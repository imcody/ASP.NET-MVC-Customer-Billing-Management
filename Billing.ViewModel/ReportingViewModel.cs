using System.Collections.Generic;

namespace Billing.ViewModel
{
    public class TicketingReportViewModel
    {
        public List<TicketingReport> TicketReport { get; set; }
        public List<TicketingReport> VoidReport { get; set; }
        public DailyReport DailyStatement { get; set; }
    }
    public class InvoicePaymentViewModel
    {
        public int Id { get; set; }
        public string AgentName { get; set; }
        public int InvoiceId { get; set; }
        public double Amount { get; set; }
        public double Received { get; set; }
        public double Adjusted { get; set; }
        public string Method { get; set; }
    }
    public class TicketingReport
    {
        public string AgentName { get; set; }
        public double Apc { get; set; }
        public double CustomerAmount { get; set; }
        public int InvoiceID { get; set; }
        public string InvDate { get; set; }
        public string PaxName { get; set; }
        public string TicketNo { get; set; }
        public double TicketTax { get; set; }
        public double VendorCharge { get; set; }
        public string VendorName { get; set; }
        public double VNetFare { get; set; }
        public int Status { get; set; }
        public int VendorId { get; set; }
        public int AAtol { get; set; }
        public int PaxType { get; set; }
        public int InvType { get; set; }
    }
    public class DailyReport
    {
        public double CashCollection { get; set; }
        public double ChequeCollection { get; set; }
        public double CCardCollection { get; set; }
        public double DCardCollection { get; set; }
        public double BankDeposit { get; set; }
        public double DailyTotal { get; set; }
    }
}