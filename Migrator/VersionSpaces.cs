using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    class VersionSpaces
    {
        private readonly SqlConnection _connection;
        public VersionSpaces(SqlConnection connection)
        { 
            _connection = connection;        
        }
        public void CreateVersion(String sqlcommand,String Vname)
        {
            _connection.Open();
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(_connection.ConnectionString);
            var createVersionTables = new SqlCommand("USE " + builder.InitialCatalog + " " + sqlcommand, _connection);
            createVersionTables.ExecuteNonQuery();
            InsertVersionTable(Vname);
            _connection.Close();
        }
        public void InsertVersionTable(string Vname)
        {
            _connection.Close();
            _connection.Open();
            DateTime now = DateTime.Now;
            var insertVersionTable = new SqlCommand(@"Insert INTO 
                                                    [Version] (Vname, CreationDate)
                                                    VALUES (@Vname, @CreationDate)", _connection);
            insertVersionTable.Parameters.AddWithValue("@Vname", Vname);
            insertVersionTable.Parameters.AddWithValue("@CreationDate", now);
            insertVersionTable.ExecuteNonQuery();
            _connection.Close();
        }
    }
}
