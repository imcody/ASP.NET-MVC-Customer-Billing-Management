
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.ViewModel
{
    public class HomeViewModel
    {
        public List<InvoiceListViewModel> InvoiceList { get; set; }
        public List<InvoiceListViewModel> DraftList { get; set; }
        public List<FloatChequeViewModel> FloatingCheques { get; set; }
        public List<FloatChequeViewModel> BulkFloatingCheques { get; set; }
    }
}