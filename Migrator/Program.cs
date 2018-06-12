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
            if (args.Contains("-cs"))
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-cs")
                    {
                        try
                        {
                            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(args[i + 1]);
                            var dbname = builder.InitialCatalog;
                            Console.WriteLine(dbname);
                        }
                        catch (Exception)
                        {

                            throw new ArgumentException("Connection string is not in a correct format");
                        }
                    
                        i = i + 1;
                    }                   
                }                
            }
            else
                throw new ArgumentException("Argument has not contain ''-cs'' paramater .");

        }
    }
}
