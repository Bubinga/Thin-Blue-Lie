using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThinBlueLieB.Helper
{
    public interface ISearches
    {
        Task<List<SimilarPerson>> SearchOfficer(string Input);
    }
}