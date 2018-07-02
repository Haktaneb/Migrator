using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    class DbController
    {
        private readonly SqlConnection _connection;
        private readonly SqlConnection _Dbconnection;
        public DbController(SqlConnection connection, SqlConnection Dbconnection)
        {
            _connection = connection;
            _Dbconnection = Dbconnection;
        }
        public void CheckDbExists(string dbname)
        {
            _connection.Open();
            var CheckDbCommand = new SqlCommand(@"SELECT name 
                                                    FROM master.dbo.sysdatabases 
                                                    WHERE ([name] = @dbname;", _connection);
            CheckDbCommand.Parameters.AddWithValue("@dbname", dbname);
            using (var reader = CheckDbCommand.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    _connection.Close();
                    Console.WriteLine("Db is exists");
                    CreateVersionTableIfNotExist();
                }
                else
                {
                    _connection.Close();
                    _connection.Open();
                    var CreateDbCommand = new SqlCommand(@"CREATE DATABASE " + dbname + ";", _connection);
                    CreateDbCommand.ExecuteNonQuery();
                    _connection.Close();
                    Console.WriteLine("Db is created");
                    CreateVersionTableIfNotExist();
                }
            }
        }
        public void CreateVersionTableIfNotExist ()
        {
            _Dbconnection.Open();
            var CreateVersionTableIfNotExistComand = new SqlCommand(@"If not exists
                                                        (select * 
                                                        from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Version') 
                                                        CREATE TABLE Version(Vname varchar(300) ,Order_ int IDENTITY(1,1) ,CreationDate datetime)", _Dbconnection);
            CreateVersionTableIfNotExistComand.ExecuteNonQuery();
            _Dbconnection.Close();
        }        
    }
}
