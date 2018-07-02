using System;
using System.Data.SqlClient;

namespace Migrator
{
    public class MigrationParameters
    {
        public SqlConnectionStringBuilder ConnectionString { get; set; }
        public string SqlScriptsBasePath { get; set; }
        public static MigrationParameters ParseArguments(string[] args)
        {
            var parameterModel = new MigrationParameters();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-cs")
                {
                    parameterModel.ConnectionString = new SqlConnectionStringBuilder(args[i + 1]);
                }

                if (args[i] == "-p")
                {
                    var path = args[i + 1];

                    if (path[path.Length - 1] != '\\') path = path + "\\";

                    parameterModel.SqlScriptsBasePath = path;
                }
            }

            parameterModel.Validate();

            return parameterModel;
        }
        private void Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString.ToString()))
            {
                throw new ArgumentException("Connection string must be specified");
            }

            if (string.IsNullOrWhiteSpace(SqlScriptsBasePath))
            {
                throw new ArgumentException("Scripts path must be specified");
            }
        }
    }
}
