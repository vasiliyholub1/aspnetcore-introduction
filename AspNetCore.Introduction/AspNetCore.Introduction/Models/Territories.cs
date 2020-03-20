using System;
using System.Collections.Generic;

namespace AspNetCore.Introduction.Models
{
    public partial class Territories
    {
        public Territories()
        {
            EmployeeTerritories = new HashSet<EmployeeTerritories>();
        }

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public virtual Regions Region { get; set; }
        public virtual ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
