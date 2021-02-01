using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ThinBlueLie.Helper.Validators
{
    public class UsernamePunctuation : RegularExpressionAttribute
    {
        public UsernamePunctuation() : base(@"^(?![-_.])[\S]+(?<![-_.])$")
        {
        }
    }
    public class RepeatedPuncutation : RegularExpressionAttribute
    {
        public RepeatedPuncutation() : base("^((?!.*[-_.]{2}).)*$")
        {
        }
    }

}
