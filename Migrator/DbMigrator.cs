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

        public DbMigrator(MigrationParameters parameters)
        {
            filesPath = parameters.SqlScriptsBasePath;
            spesificVersionNumber = parameters.SpesificVersionNumber;
            dbName = parameters.ConnectionString.InitialCatalog;
            parameters.ConnectionString.InitialCatalog = "master";
            connection = new SqlConnection(parameters.ConnectionString.ConnectionString);
            connection.Open();
        }

        public void Migrate()
        {
            var dbController = new DbController(dbName, connection);
            dbController.CreateDatabase();

            var updater = new DbVersionUpdater(spesificVersionNumber,filesPath, connection);
            if (spesificVersionNumber != 0)
            {
                updater.UpdateDbWithSpesificVersionNumber();
            }
            updater.UpdateDb();
            
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
