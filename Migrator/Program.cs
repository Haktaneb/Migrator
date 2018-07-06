using Migrator.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            List<FileContent> fileNameList = new List<FileContent>();

            var parameters = MigrationParameters.ParseArguments(args);

            using (DbMigrator migrator = new DbMigrator (parameters))
            {
                migrator.Migrate();
            } 

        }
    }
}
