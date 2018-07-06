namespace Migrator
{
    using Microsoft.SqlServer.Management.Common;
    using Microsoft.SqlServer.Management.Smo;
    using Migrator.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.IO;
    using System.Linq;

    public class DbVersionUpdater
    {
        private readonly SqlConnection connection;
        private readonly int spesificVersionNumber;
        private readonly List<FileVersionModel> scriptFiles;

        internal DbVersionUpdater(int spesificVersionNumber , List<FileVersionModel> scriptFiles, SqlConnection connection)
        {
            this.spesificVersionNumber = spesificVersionNumber;
            this.connection = connection;          
            this.scriptFiles = scriptFiles.Where(e => e.IsUp.Equals(true)).ToList();
        }

        public void UpdateDb()
        {
            var v = GetDbCurrentVersion();
            var fileVersion = GetFileCurrentVersion();
            int versionNumberFinder = 0;

            if (fileVersion > v)
            {
                foreach (var file in scriptFiles)
                {
                    versionNumberFinder++;
                    if (v < versionNumberFinder)
                    {
                        RunScriptFile(file.Path);
                        InsertVersion(file.Name);
                    }                  
                }
            }            
        }
        public void UpdateDbWithSpesificVersionNumber()
        {
            var v = GetDbCurrentVersion();
            var fileVersion = GetFileCurrentVersion();
            int versionNumberFinder = 0;
             
            if (fileVersion >= spesificVersionNumber && fileVersion>v)
            {
                if (spesificVersionNumber - 1 != v)
                {
                    foreach (var file in scriptFiles)
                    {
                        versionNumberFinder++;
                        if (versionNumberFinder == spesificVersionNumber)
                        {
                            break;
                        }
                       else if (v <= spesificVersionNumber && v < versionNumberFinder)
                        {
                            RunScriptFile(file.Path);
                            InsertVersion(Path.GetFileNameWithoutExtension(file.Name));
                        }
                        
                    }
                }
                else
                {
                    RunScriptFile(scriptFiles[spesificVersionNumber - 1].Path);
                    InsertVersion(scriptFiles[spesificVersionNumber - 1].Name);
                }              
            }
            else
            {
                throw new ArgumentException("Version number doesn't match");
           }      
        }

        public int GetDbCurrentVersion()
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

        public int GetFileCurrentVersion()
        {
           var FileVersion = scriptFiles[scriptFiles.Count-1].Version;
            return FileVersion;
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

        public void RunScriptFile(string path)
        {
            System.Diagnostics.Debug.WriteLine($"Running Script From {path}");

            string script = File.ReadAllText(path);
            
            Server server = new Server(new ServerConnection(connection));

            server.ConnectionContext.ExecuteNonQuery(script);
        }
    }
}
