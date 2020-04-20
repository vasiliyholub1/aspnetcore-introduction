using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Introduction.Models
{
    public class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

        [Required]
        public int CategoryId { get; set; }
        
        [Display(Name = "Category name")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        [StringLength(15)]
        [Required]
        public string CategoryName { get; set; }

        [Display(Name = "Description")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        public string Description { get; set; }

        [Display(Name = "Picture")]
        public byte[] Picture { get; set; }

        public virtual ICollection<Products> Products { get; set; }
    }
}
