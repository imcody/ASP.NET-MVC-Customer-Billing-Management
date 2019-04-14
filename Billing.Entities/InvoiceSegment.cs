using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Billing.Entities
{
    public class InvoiceSegment
    {
        [Key]
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
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
        public string SegmentSecondaryStatus { get; set; }
        public string FlightDate { get; set; }

    }
}