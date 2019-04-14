using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.DAL.Parameters
{
    public class FinalizeFareInfoUpdate
    {
        public int InvoiceId { get; set; }
        public string ApplicationUserId { get; set; }
        public string Remarks { get; set; }
        public double Total { get; set; }
    }
    public class InvoicePaymentCashVoucher
    {
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserId { get; set; }
        public int AgentId { get; set; }
        public int InvoiceId { get; set; }
    }
    public class SegmentUpdateParams
    {
        public int TableId { get; set; }
        public int InvoiceId { get; set; }
        public string AirlinesCode { get; set; }
        public string FlightNo { get; set; }
        public string SegmentClass { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureFrom { get; set; }
        public string DepartureTo { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public string SegmentStatus { get; set; }
        public string FlightDate { get; set; }
    }
}