using AspNetCore.Introduction.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCore.Introduction.ViewModels
{
    public class ProductCreationViewModel
    {
        public Products Product { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Suppliers { get; set; }
    }
}