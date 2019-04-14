using System;
using System.ComponentModel.DataAnnotations;

namespace Billing.Entities
{
    public class InvoiceName
    {
        [Key]
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int InvoiceId { get; set; }
        public DateTime? BookingDate { get; set; }
        public PassengerType? PassengerTypes { get; set; }
        public string Name { get; set; }
        public string TicketNo { get; set; }
        public double Amount { get; set; }
        public double CNetFare { get; set; }
        public double VNetFare { get; set; }
        public double TicketTax { get; set; }
        public double VendorCharge { get; set; }
        public double Apc { get; set; } //Atol Protection Charge
        public int? Status { get; set; }
    }
}