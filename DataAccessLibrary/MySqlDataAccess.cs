﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace DataAccessLibrary
{
    public class MySQLDataAccess
    {
        public List<T> LoadData<T, U>(string sqlStatement, U parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();
                return rows;
            }


        }

        public void SaveData<T>(string sqlStatement, T parameters, string connectionString)
        {
            using (IDbConnection connection = new MySqlConnection(connectionString))
            {
                connection.Execute(sqlStatement, parameters);
            }
        }




    }
}
