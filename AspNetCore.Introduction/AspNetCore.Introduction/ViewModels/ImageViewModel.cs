using AspNetCore.Introduction.Models;

namespace AspNetCore.Introduction.ViewModels
{
    public class ImageViewModel
    {
        public Categories Category { get; set; }
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
    }
}