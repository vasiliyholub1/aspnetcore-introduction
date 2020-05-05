using System.ComponentModel.DataAnnotations;

namespace AspNetCore.Introduction.ViewModels
{
    public class UploadImageViewModel
    {
        [Required]
        public byte[] File { get; set; }
    }
}