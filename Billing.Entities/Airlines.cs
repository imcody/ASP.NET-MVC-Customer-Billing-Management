using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Billing.Entities
{
    public class Airlines
    {
        [Key]
        public int Id { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(150), MinLength(6)]
        public string Name { get; set; }
        [Index(IsUnique = true)]
        [MaxLength(20), MinLength(2)]
        public string Code { get; set; }
    }
}