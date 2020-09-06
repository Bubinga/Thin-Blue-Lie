using DataAccessLibrary.DataModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThinBlueLieB.Searches
{
    public interface ISearchesTimeline
    {
        Task<List<List<Timelineinfo>>> GetTimeline(string current, int? dateChange, string date);
    }
}