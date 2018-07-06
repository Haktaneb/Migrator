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
    internal class DbVersionReducer : DbVersionUpdater
    {
        private readonly SqlConnection connection;
        private readonly int spesificVersionNumber;
        private readonly List<FileVersionModel> scriptFiles;

        internal  DbVersionReducer(int spesificVersionNumber, List<FileVersionModel> scriptFiles, SqlConnection connection) : base(spesificVersionNumber,scriptFiles,connection)
        {
            this.spesificVersionNumber = spesificVersionNumber;
            this.connection = connection;
            this.scriptFiles = scriptFiles.Where(e => e.IsUp.Equals(false)).ToList(); ;
        }

        public void ReduceDb()
        {
            
            
            var dbVersion = GetDbCurrentVersion();
            var fileVersion = GetFileCurrentVersion();
            int versionNumberFinder = 0;

            if ( spesificVersionNumber<=dbVersion)
            {
                foreach (var file in scriptFiles)
                {
                    versionNumberFinder++;
                    if (versionNumberFinder >= spesificVersionNumber)
                    {
                        RunScriptFile(file.Path);
                        DeleteVersion(file.Name);
                    }
                }
            }
        }
        public void DeleteVersion(string version)
        {
            var deleteVersion = new SqlCommand(@"DELETE FROM 
                                                    [Version] (Vname)
                                                    VALUES (@Vname)
                                                    Where Vname = @Vname", connection);

            deleteVersion.Parameters.AddWithValue("@Vname", version);
            deleteVersion.ExecuteNonQuery();
        }
    }
}
