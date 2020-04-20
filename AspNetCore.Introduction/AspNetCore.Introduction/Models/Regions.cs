using System.Collections.Generic;

namespace AspNetCore.Introduction.Models
{
    public partial class Regions
    {
        public Regions()
        {
            Territories = new HashSet<Territories>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        public virtual ICollection<Territories> Territories { get; set; }
    }
}
