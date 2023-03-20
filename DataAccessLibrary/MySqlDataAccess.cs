using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Security;

namespace DataAccessLibrary
{
    public class MySQLDataAccess : IDataAccess
    {
        private readonly IConfiguration _config;

        public MySQLDataAccess(IConfiguration config)
        {
            _config = config;

        }

        public async Task<List<T>> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName, bool isStoredProcedure = false)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);

            CommandType commandType = CommandType.Text;

            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }


            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                var rows = await connection.QueryAsync<T>(sqlStatement, parameters, commandType: commandType);
                return rows.ToList();
            }


        }

        public async Task SaveData<T>(string sqlStatement, T parameters, string connectionStringName, bool isStoredProcedure = false)
        {

            string connectionString = _config.GetConnectionString(connectionStringName);
            
            CommandType commandType = CommandType.Text;

            if (isStoredProcedure == true)
            {
                commandType = CommandType.StoredProcedure;
            }

            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                await connection.ExecuteAsync(sqlStatement, parameters, commandType: commandType);
            }
        }




    }
}
