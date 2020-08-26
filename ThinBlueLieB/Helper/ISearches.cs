using DataAccessLibrary.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using ThinBlueLieB.Models;

namespace ThinBlueLieB.Helper
{
    public interface ISearches
    {
        Task<List<SimilarPerson>> SearchOfficer(string Input);
        Task<List<ViewSimilar>> GetSimilar(string? TempDate);
        Task<List<List<Timelineinfo>>> GetTimeline(string? current, int? dateChange, string? date);
    }
}
