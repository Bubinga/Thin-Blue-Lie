using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinBlueLieB.Helper;


namespace ThinBlueLieB.Components.Bases
{
    public class SimilarPeopleBase : ComponentBase
    {
        [Parameter]
        public string InputName { get; set; }
        public string Input = "test";
        public List<SimilarPerson> People = new List<SimilarPerson>();
        protected async Task<List<SimilarPerson>> GetPeople(string InputName)
        {
          //  People = await Searches.SearchOfficer(Input);
            return People;
        }         
    }
}
