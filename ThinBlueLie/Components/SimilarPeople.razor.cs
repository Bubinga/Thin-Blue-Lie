using System.Collections.Generic;
using DataAccessLibrary.DataModels;
using Microsoft.AspNetCore.Components;
using ThinBlueLie.Models;
using static ThinBlueLie.Searches.SearchClasses;

namespace ThinBlueLie.Components
{
    public partial class SimilarPeople
    {
        [Parameter]
        public string TempDate { get; set; }

        [Parameter]
        public List<SimilarPersonGeneral> People { get; set; } = null;
        [Parameter]
        public int PersonRank { get; set; }

        [Parameter]
        public EventCallback<SimilarPeopleModel> SetSameAs { get; set; }

        public class SimilarPeopleModel : CommonPerson
        {
            public int IdPerson { get; set; }

            public int PersonRank { get; set; }
        }
    }
}