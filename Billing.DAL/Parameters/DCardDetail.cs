using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Billing.DAL.Parameters
{
    public class DCardDetail
    {
        [Key]
        public int Id { get; set; }
        public string Notes { get; set; }
        public double Amount { get; set; }
        public int InvoiceId { get; set; }
        public int BankAccountId { get; set; }
        public string CardNo { get; set; }
        public string CardHolder { get; set; }
        public string ExtraAmount { get; set; }
        public string BankDate { get; set; }
        public string UserId { get; set; }
    }
}