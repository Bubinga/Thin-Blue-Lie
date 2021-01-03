using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Models.ViewModels
{
    public class FirstLoadEvents
    {
        public int IdTimelineinfo { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Context { get; set; }
        public ViewMedia Media { get; set; }
    }
}
