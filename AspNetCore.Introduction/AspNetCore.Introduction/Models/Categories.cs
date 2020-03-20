using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Introduction.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        public int CategoryId { get; set; }
        
        [Display(Name = "Category name")]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
