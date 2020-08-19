using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess
{
    public interface IUserAccess
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters, string connectionString);
        Task SaveData<T>(string sql, T parameters, string connectionString);
    }
}