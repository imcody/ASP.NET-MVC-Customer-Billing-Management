
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.ViewModel
{
    public class OtherInvocieTypeList
    {
        public int Id { get; set; }
        public string InvoiceType { get; set; }
        public string CreatedOn { get; set; }
        public string UserId { get; set; }
    }
    public class OtherInvoiceList
    {
        public double ACAmount { get; set; }
        public string Agent { get; set; }
        public string InvoiceType { get; set; }
        public string CreatedOn { get; set; }
        public double Due { get; set; }
        public int InvoiceId { get; set; }
        public double Paid { get; set; }
        public double VAmount { get; set; }
        public double VDue { get; set; }
        public string Vendor { get; set; }
        public double VPaid { get; set; }
        public string User { get; set; }
    }
    public class OtherInvoiceDetailsViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceType { get; set; }
        public int InvType { get; set; }
        public string CreationDate { get; set; }
        public string Reference { get; set; }
        public string EDP { get; set; }
        public string VInvNo { get; set; }
        public string Details { get; set; }
        public string PersonName { get; set; }
        public double ACAmount { get; set; }
        public double VAmount { get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }
        public string AgentName { get; set; }
        public string AgentAddress { get; set; }
        public string AgentEmail { get; set; }
        public string AgentPhone { get; set; }
        public string VendorName { get; set; }
        public string VendorAddress { get; set; }
        public string VendorEmail { get; set; }
        public string VendorPhone { get; set; }
        public List<InvoiceDetailsLogViewModel> InvLogs { get; set; }
        public List<OtherInvoiceDetailsPaymentViewModel> InvPayments { get; set; }
    }
    public class OtherInvoiceDetailsPaymentViewModel
    {
        public string PaymentDate { get; set; }
        public string PersonName { get; set; }
        public string Remarks { get; set; }
        public double Amount { get; set; }
    }
}