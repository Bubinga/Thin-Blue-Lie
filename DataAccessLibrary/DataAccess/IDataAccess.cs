using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLibrary.DataAccess
{
    public interface IDataAccess
    {
        Task<List<T>> LoadData<T, U>(string sql, U parameters);
        Task<List<T>> LoadDataNoLog<T, U>(string sql, U parameters, string connectionString);
        Task<T> LoadDataSingle<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
        Task<int> SaveDataAndReturn<T>(string sql, T parameters);
    }
}