using System.Collections.Generic;

namespace ThinBlueLie.ViewModels
{
    public class SimpleTimelineinfo
    {
        public string Date { get; set; }
        public string City { get; set; }
        public int State { get; set; }
    }
    public class Person
    {
        public string Name { get; set; }
        public int Race { get; set; }
        public int Sex { get; set; }
        public List<SimpleTimelineinfo> Events { get; set; }
    }
}
