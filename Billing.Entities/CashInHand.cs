using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Billing.Entities
{
    public class CashInHand
    {
        [Key]
        public int Id { get; set; }
        public double Balance { get; set; }
    }
}