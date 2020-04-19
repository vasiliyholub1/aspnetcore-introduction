using System.Collections.Generic;
using AspNetCore.Introduction.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Introduction.ViewModels
{
    public class ProductCategoryViewModel
    {
        public List<Products> Products { get; set; }
        public SelectList Categories { get; set; }

        public string ProductCategory { get; set; }

        public string SearchString { get; set; }
    }
}