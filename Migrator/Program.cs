using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
          
            foreach (var item in args)
            {
                if (item.Contains("Initial Catalog"))
                {
                    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(item);
                    var dbname = builder.InitialCatalog;
                    Console.WriteLine(dbname);
                }
            }
           
            

        }
    }
}
