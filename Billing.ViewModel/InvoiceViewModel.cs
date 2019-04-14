using Billing.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Billing.ViewModel
{
    public class InvoiceDetailsViewModel
    {
        [Key]
        public int InvoiceId { get; set; }
        public int InvType { get; set; }
        public DateTime SysCreateDate { get; set; }
        public string AgentName { get; set; }
        public string Pnr { get; set; }
        public string GdsBookingDate { get; set; }
        public string AirlinesName { get; set; }
        public string VendorName { get; set; }
        public string VendorInvNo { get; set; }
        public DateTime ExpectedPaymentDate { get; set; }
        public GDS GDSs { get; set; }
        public string AgentAddress { get; set; }
        public string AgentPostCode { get; set; }
        public string AgentTelephone { get; set; }
        public string AgentMobile { get; set; }
        public string AgentFax { get; set; }
        public string AgentEmail { get; set; }
        public string CancellationChargeBefore { get; set; }
        public string CancellationChargeAfter { get; set; }
        public string CancellationDateBefore { get; set; }
        public string CancellationDateAfter { get; set; }
        public string NoShowBefore { get; set; }
        public string NoShowAfter { get; set; }
        public double ExtraCharge { get; set; }
        public double AmountReceived { get; set; }
        public string PersonName { get; set; }
        public string CreatedBy { get; set; }
        public List<InvoiceDetailsPaxViewModel> PaxList { get; set; }
        public List<InvoiceDetailsSegmentViewModel> invSegments { get; set; }
        public List<InvoiceDetailsLogViewModel> invLog { get; set; }
        public List<InvoicePaymentHistory> invPaid { get; set; }
        public double Total { get; set; }
    }
    public class InvoiceBookingViewModel
    {
        [Key]
        public int InvoiceId { get; set; }
        public DateTime SysCreateDate { get; set; }
        public ProfileType InvoiceType { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public string AgentPostCode { get; set; }
        public string AgentTelephone { get; set; }
        public string AgentMobile { get; set; }
        public string AgentFax { get; set; }
        public string AgentEmail { get; set; }
        public GDS GDSs { get; set; }
        public string Pnr { get; set; }
        public string GdsBookingDate { get; set; }
        public int AirlinesId { get; set; }
        public string AirlinesName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string VendorInvNo { get; set; }
        public DateTime? ExpectedPaymentDate { get; set; }
        public List<InvoiceSegment> Segments { get; set; }
        public List<InvoiceName> PaxList { get; set; }
        public double? CancellationChargeBefore { get; set; }
        public double? CancellationChargeAfter { get; set; }
        public string CancellationDateBefore { get; set; }
        public string CancellationDateAfter { get; set; }
        public double? NoShowBefore { get; set; }
        public double? NoShowAfter { get; set; }
        public double ExtraCharge { get; set; }
    }
    public class InvoiceListViewModel
    {
        [Key]
        public int InvoiceId { get; set; }
        public DateTime SysCreateDate { get; set; }
        public string AirlineCode { get; set; }
        public string AgentCust { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public double Refund { get; set; }
        public double Due { get; set; }
        public string PersonName { get; set; }
    }
    public class UpdateFareInfo
    {
        [Key]
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public PassengerType PassengerTypes { get; set; }
        public string Name { get; set; }
        public string TicketNo { get; set; }
        public double Amount { get; set; }
        public double Tax { get; set; }
        public double Safi { get; set; }
        public double Apc { get; set; }
        public double CNetFare { get; set; }
        public double VNetFare { get; set; }
        public double VendorCharge { get; set; }
        public double Profit { get; set; }
    }
    public class InvoicePrintViewModel
    {
        public CompanyInfo CompInfo { get; set; }
        public InvoiceDetailsViewModel InvDetails { get; set; }
        public double Total { get; set; }
    }
    public class InvoiceDetailsPaxViewModel
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int InvoiceId { get; set; }
        public string PaxType { get; set; }
        public string Name { get; set; }
        public string TicketNo { get; set; }
        public string BookingDate { get; set; }
        public double Fare { get; set; }
        public double Tax { get; set; }
        public double Amount { get; set; }
    }
    public class InvoiceDetailsSegmentViewModel
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
    public class InvoiceDetailsLogViewModel
    {
        public string Remarks { get; set; }
        public string UserID { get; set; }
        public string Time { get; set; }
        public string Date { get; set; }
    }
}