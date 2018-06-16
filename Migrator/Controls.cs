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
        private readonly SqlConnection _Dbconnection;

        public Controls(SqlConnection connection, SqlConnection Dbconnection)
        {
            _connection = connection;
            _Dbconnection = Dbconnection;
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
                    CheckTable();

                }
                else
                {
                    _connection.Close();
                    _connection.Open();

                    var CreateDbCommand = new SqlCommand(@"CREATE DATABASE " + dbname + ";", _connection);
                    CreateDbCommand.ExecuteNonQuery();

                    _connection.Close();

                    Console.WriteLine("Db is created");
                    CheckTable();

                }


            }
        }
        public void CheckTable()
        {
            _Dbconnection.Open();

            var CheckTableComand = new SqlCommand(@"If not exists
                                                        (select * 
                                                        from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Version') 
                                                        CREATE TABLE Version(Vname varchar(30) ,Order_ int ,CreationDate datetime)", _Dbconnection);
            CheckTableComand.ExecuteNonQuery();

            _Dbconnection.Close();

        }
    }
}
