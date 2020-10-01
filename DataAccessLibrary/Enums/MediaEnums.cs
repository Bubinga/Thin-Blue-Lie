using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Enums
{
    public class MediaEnums
    {
        public enum MediaTypeEnum
        {
            Video = 1,            
            Image,
            [Display(Name = "News Article")]
            News,  
        }
        public enum SourceFromEnum
        {
            Youtube = 1,
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
