using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    class Controls
    {
        private readonly SqlConnection _connection;

        public Controls(SqlConnection connection)
        {
            _connection = connection;

        }
        public void CheckDbExists(string dbname)
        {
            _connection.Open();
            var CheckDbCommand = new SqlCommand(@"SELECT name 
                                                    FROM master.dbo.sysdatabases 
                                                    WHERE ('[' + name + ']' = @dbname 
                                                    OR name = @dbname)", _connection);

            CheckDbCommand.Parameters.AddWithValue("@dbname", dbname);



            using (var reader = CheckDbCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    _connection.Close();
                    Console.WriteLine("Db is exists");

                }
                else
                {
                    _connection.Close();

                    _connection.Open();

                    var CreateDbCommand = new SqlCommand(@"CREATE DATABASE " + dbname+ ";", _connection);
                    CreateDbCommand.Parameters.AddWithValue("@dbname", dbname);

                    CreateDbCommand.ExecuteNonQuery();

                    _connection.Close();
                    Console.WriteLine("Db is created");
                }


            }
        }

    }
}
