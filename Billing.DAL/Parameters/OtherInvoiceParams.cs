using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Billing.DAL.Parameters
{
    public class CreateOtherInvoice
    {
        public int AgentId { get; set; }
        public int VendorId { get; set; }
        public int OtherInvoiceTypeId { get; set; }
        public string Reference { get; set; }
        public string ExpectedPayDate { get; set; }
        public string VendorInvNo { get; set; }
        public string Details { get; set; }
        public double CustomerAgentAmount { get; set; }
        public double VendorAmount { get; set; }
    }
}