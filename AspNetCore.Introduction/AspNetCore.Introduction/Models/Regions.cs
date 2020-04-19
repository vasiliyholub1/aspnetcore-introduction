using System;
using System.Collections.Generic;

namespace AspNetCore.Introduction.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Regions
    {
        public Regions()
        {
            Territories = new HashSet<Territories>();
        }

        [Key]
        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }
}
