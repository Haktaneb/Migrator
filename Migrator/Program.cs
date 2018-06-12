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
            Boolean control=false;
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] == "-cs")
                    {
                     control = true;
                        try
                        {
                            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(args[i + 1]);
                            var dbname = builder.InitialCatalog;
                            Console.WriteLine(dbname);
                        }
                        catch (ArgumentException e )
                        {
                            Console.WriteLine("Connection string is not in a correct format",e);                   
                        }

                              i++;
                    }                   
                }
            if (control == false)
            {             
                Console.WriteLine("Argument has not contain ''- cs'' paramater.");
            }

        }
    }
}
