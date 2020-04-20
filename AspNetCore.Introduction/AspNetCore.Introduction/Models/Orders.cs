using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Introduction.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
        [Display(Name = "Order date"), DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        [Display(Name = "Required date"), DataType(DataType.Date)]
        public DateTime? RequiredDate { get; set; }
        [Display(Name = "Shipped date"), DataType(DataType.Date)]
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$"), StringLength(40)]
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Employees Employee { get; set; }
        public virtual Shippers ShipViaNavigation { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
