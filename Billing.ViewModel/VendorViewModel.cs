
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Billing.ViewModel
{
    public class CreateVendorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string FaxNo { get; set; }
        [Display(Name = "Safi")]
        public double NetSafi { get; set; }
        public double Atol { get; set; }
        public double Balance { get; set; }
    }
    public class VendorListViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        [Display(Name = "Fax No")]
        public string FaxNo { get; set; }
        [Display(Name = "Safi")]
        public double NetSafi { get; set; }
        public double Balance { get; set; }
    }
    public class EditVendorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        [Display(Name = "Fax No")]
        public string FaxNo { get; set; }
        [Display(Name = "Safi")]
        public double NetSafi { get; set; }
        public double Atol { get; set; }
        //public double Balance { get; set; }
    }
    public class VendorLedgerViewModel
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
    public class VendorOutstandingInvoiceListViewModel
    {
        public int InvoiceId { get; set; }
        public string InvoiceDate { get; set; }
        public double Total { get; set; }
        public double Paid { get; set; }
        public double Due { get; set; }

    }
    public class InvoicePaymentHistoryVendor
    {
        public int InvoiceId { get; set; }
        public string PaymentDate { get; set; }
        public string UserName { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
        public string PaymentMethod { get; set; }
    }
}