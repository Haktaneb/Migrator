using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    public class DbMigrator : IDisposable
    {
        bool disposed = false;

        private readonly SqlConnection connection;
        private readonly string dbName;
        private readonly string filesPath;
        private readonly int spesificVersionNumber;
        private readonly bool isUp;

        public DbMigrator(MigrationParameters parameters)
        {
            filesPath = parameters.SqlScriptsBasePath;
            spesificVersionNumber = parameters.SpesificVersionNumber;
            dbName = parameters.ConnectionString.InitialCatalog;
            isUp = parameters.IsUp;
            parameters.ConnectionString.InitialCatalog = "master";
            connection = new SqlConnection(parameters.ConnectionString.ConnectionString);
            connection.Open();
        }

        public void Migrate()
        {
            var dbController = new DbController(dbName, connection);
            dbController.CreateDatabase();

            var fileOperator = new FileOperations();
            fileOperator.FileReader(filesPath);
            var xxx = fileOperator.GetFileVersions();
            if (isUp == true)
            {
                var updater = new DbVersionUpdater(spesificVersionNumber, fileOperator.GetFileVersions(), connection);
                if (spesificVersionNumber != 0)
                {
                    updater.UpdateDbWithSpesificVersionNumber();
                }
                else
                {
                    updater.UpdateDb();
                }
            }
            else
            {
                var reducer = new DbVersionReducer(spesificVersionNumber, fileOperator.GetFileVersions(), connection);
                reducer.ReduceDb();                             
            }               
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                connection.Dispose();
            }

            disposed = true;
        }

    }
}
