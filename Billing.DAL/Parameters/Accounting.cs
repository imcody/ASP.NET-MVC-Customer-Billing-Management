using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Billing.DAL.Parameters
{
    public class GeneralVoucherPosting
    {
        public int LedgerHeadId { get; set; }
        public double Amount { get; set; }
        public string Notes { get; set; }
        public string UserID { get; set; }
        public string LedgerDate { get; set; }
    }
}