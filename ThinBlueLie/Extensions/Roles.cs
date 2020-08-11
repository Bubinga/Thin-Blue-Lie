using System;
using System.Collections.Generic;

namespace ThinBlue
{
    public partial class Roles
    {
        public Roles()
        {
            Userroles = new HashSet<Userroles>();
        }

        public string Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Userroles> Userroles { get; set; }
    }
}
