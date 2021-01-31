extern alias MySqlConnectorAlias;

using MySqlConnectorAlias::MySqlConnector;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.Logging;
//using MySqlConnectorAlias::MySql.Data.MySqlClient;

namespace DataAccessLibrary.DataAccess
{
    public class DataAccess : IDataAccess
    {
        private readonly IDbConnectionFactory connectionFactory;
        public DataAccess(IDbConnectionFactory _connectionFactory)
        {
            connectionFactory = _connectionFactory;
        }

        public async Task<List<T>> LoadData<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows.ToList();
            }
        }
        public async Task<List<T>> LoadDataNoLog<T, U>(string sql, U parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sql, parameters);
                return rows.ToList();
            }
        }

        public async Task<T> LoadDataSingle<T, U>(string sql, U parameters)
        {
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                var row = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                return row;
            }
        }

        public async Task SaveData<T>(string sql, T parameters)
        {
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                await connection.ExecuteAsync(sql, parameters);
            }
        }

        public async Task<int> SaveDataAndReturn<T>(string sql, T parameters)
        {
            using (IDbConnection connection = connectionFactory.CreateConnection())
            {
                int rows = await connection.ExecuteAsync(sql, parameters);
                return rows;
            }
        }
    }
}
