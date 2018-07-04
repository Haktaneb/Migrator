namespace Migrator
{
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;

    public class DbVersionUpdater
    {
        private readonly SqlConnection connection;
        private readonly int spesificVersionNumber;
        private readonly string[] scriptFiles;

        public DbVersionUpdater(int spesificVersionNumber ,string fileDirectoryPath, SqlConnection connection)
        {
            this.spesificVersionNumber = spesificVersionNumber;
            this.connection = connection;
            scriptFiles = Directory.GetFiles(fileDirectoryPath, "*.txt");
        }

        public void UpdateDb()
        {
            var v = GetDbCurrentVersion();
            var fileV = GetFileCurrentVersion();
            int versionNumberFinder = 0;

            if (fileV > v)
            {
                foreach (var file in scriptFiles)
                {
                    versionNumberFinder++;
                    if (v < versionNumberFinder)
                    {
                        RunScriptFile(file);
                        InsertVersion(Path.GetFileNameWithoutExtension(file));
                    }                  
                }
            }            
        }
        public void UpdateDbWithSpesificVersionNumber()
        {
            var v = GetDbCurrentVersion();
            var fileV = GetFileCurrentVersion();
            if (fileV >= spesificVersionNumber)
            {
                RunScriptFile(scriptFiles[spesificVersionNumber - 1]);
                InsertVersion(Path.GetFileNameWithoutExtension(scriptFiles[spesificVersionNumber - 1]));
            }
            else
            {
                throw new ArgumentException("Version number doesn't match");
           }      
        }

        private int GetDbCurrentVersion()
        { 
            var sql = @"SELECT TOP 1 [Vname] FROM [Version] ORDER BY CreationDate DESC";

            var getCurrentVersionCommand = new SqlCommand(sql, connection);
            var version = getCurrentVersionCommand.ExecuteScalar();

            if(version == null)
            {
                return -1; 
            }

            return GetVersionNumberFromVersionName(version.ToString());
        }

        private int GetFileCurrentVersion()
        {
           var FileVersion = scriptFiles[scriptFiles.Length-1];
           return GetVersionNumberFromVersionName(Path.GetFileNameWithoutExtension(FileVersion));
        }

        private int GetVersionNumberFromVersionName(string fileName)
        {
            int index = fileName.IndexOf('_');
            var versionStr = index == -1 ? fileName : fileName.Substring(0, index).Remove(0,1);
            return Convert.ToInt32(versionStr);
        }

        private void InsertVersion(string version)
        {
            var insertVersion = new SqlCommand(@"Insert INTO 
                                                    [Version] (Vname, CreationDate)
                                                    VALUES (@Vname, @CreationDate)", connection);

            insertVersion.Parameters.AddWithValue("@Vname", version);
            insertVersion.Parameters.AddWithValue("@CreationDate", DateTime.Now);
            insertVersion.ExecuteNonQuery();
        }

        private void RunScriptFile(string path)
        {
            System.Diagnostics.Debug.WriteLine($"Running Script From {path}");

            string script = File.ReadAllText(path);
            
            Server server = new Server(new ServerConnection(connection));

            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}
