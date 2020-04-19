using System;
using System.Collections.Generic;

namespace AspNetCore.Introduction.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Shippers
    {
        public Shippers()
        {
            Orders = new HashSet<Orders>();
        }

        [Key]
        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Orders> Orders { get; set; }
    }
}
