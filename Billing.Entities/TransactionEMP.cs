using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Billing.Entities
{
    public class TransactionEMP
    {
        [Key]
        public int Id { get; set; }
        public virtual Invoice Invoices { get; set; }
        public int InvoiceId { get; set; }
        public int tan_type { get; set; }
        public string SysDate { get; set; }
        public string SysUser { get; set; }
        public string payment_type { get; set; }
        public double Amount { get; set; }
        public double ex_amt { get; set; }
        public int bank_id { get; set; }
        public int chq_no { get; set; }
        public int acc_no { get; set; }
        public int sort_code { get; set; }
        public string card_holder_name { get; set; }
        public string card_no { get; set; }
        public int adjust_inv_no { get; set; }
        public string remark { get; set; }
        public string bank_dt { get; set; }
        public string bank_chrg { get; set; }
        public string dep_pur { get; set; }
    }
}