using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Introduction.ViewModels
{
    public class ImageViewModel
    {
        [Required]
        public HttpPostedFileBase File { get; set; }
    }
}