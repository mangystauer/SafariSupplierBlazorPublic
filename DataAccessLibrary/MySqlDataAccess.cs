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

        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionStringName)
        {
            string connectionString = _config.GetConnectionString(connectionStringName);


            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }


        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionStringName)
        {

            string connectionString = _config.GetConnectionString(connectionStringName);

            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }




    }
}
