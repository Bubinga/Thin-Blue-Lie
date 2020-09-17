using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ThinBlueLieB.Models
{
    public class ViewMedia
    {
        public int IdMedia { get; set; }
        public int IdTimelineinfo { get; set; }
        public int ListIndex { get; set; } //becomes Rank
        [Required]
        public byte MediaType { get; set; }
        [DataType(DataType.Url)]
        public string SourcePath { get; set; } //For linked image 
        public IFormFile Source { get; set; } //For uploaded image file
        [Required]
        public byte Gore { get; set; }
        [Required]
        public byte SourceFrom { get; set; }
        [Required]
        [MaxLength(250)]
        public string Blurb { get; set; }        
        public string Credit { get; set; }
    }
}
