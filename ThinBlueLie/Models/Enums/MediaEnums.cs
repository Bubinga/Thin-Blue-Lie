using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Models
{
    public class MediaEnums
    {
        public enum MediaTypeEnum
        {
            Video,            
            Image,
            [Display(Name = "News Article")]
            News,  
        }
        public enum SourceFromEnum
        {
            Youtube,
            Reddit,
            Instagram,
            Facebook,
            Twitter,
            [Display(Name = "News Website")]
            NewsSite,
            [Display(Name = "Phone or Computer")]
            Device,           
        }
    }
}
