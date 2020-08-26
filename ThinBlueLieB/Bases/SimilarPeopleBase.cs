using DataAccessLibrary.DataAccess;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;


namespace ThinBlueLieB.Components.Bases
{
    public class SimilarPeopleBase : ComponentBase
    {
       
        [Parameter]
        public string TempDate { get; set; }
        
    }
}
