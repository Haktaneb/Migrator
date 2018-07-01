using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Common;

namespace Migrator
{
    class DbController
    {
        private readonly SqlConnection connection;
        private readonly string dbName;

        public DbController(string dbName, SqlConnection connection)
        {
            this.dbName = dbName;
            this.connection = connection;
        }   

        public void CreateDatabase()
        {
            CreateDatabaseIfNotExist();
            CreateVersionTableIfNotExist();
        }

        private void CreateDatabaseIfNotExist()
        {
            var CheckDbCommand = new SqlCommand(@"SELECT count(*) FROM sysdatabases WHERE [name] = @dbname", connection);

            CheckDbCommand.Parameters.AddWithValue("@dbname", dbName);

            var count = Convert.ToInt32 (CheckDbCommand.ExecuteScalar());

            var isDbExists = count > 0;

            if (!isDbExists)
            {
                var CreateDbCommand = new SqlCommand($"CREATE DATABASE {dbName};", connection);
                CreateDbCommand.ExecuteNonQuery();
            }
            
            connection.ChangeDatabase(dbName);
        }

        private void CreateVersionTableIfNotExist()
        {
            var CreateVersionTableIfNotExistComand = new SqlCommand(@"If not exists
                                                        (select * 
                                                        from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Version') 
                                                        CREATE TABLE Version(Vname varchar(300) ,Order_ int IDENTITY(1,1) ,CreationDate datetime)", connection);
            CreateVersionTableIfNotExistComand.ExecuteNonQuery();
        }

    }
}
