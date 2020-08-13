using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ThinBlueLie.ViewModels
{
    public class ViewMedia
    {
        public int IdMedia { get; set; }
        public int IdTimelineinfo { get; set; }
        public byte MediaType { get; set; }
        public string SourcePath { get; set; } //For linked image 
        public IFormFile Source { get; set; } //For uploaded image file
        public byte Gore { get; set; }
        public byte SourceFrom { get; set; }
        [Required]
        [MaxLength(250)]
        public string Blurb { get; set; }
    }
}
