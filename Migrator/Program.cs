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
            SqlConnectionStringBuilder builder;
            SqlConnection cnn = null;
            SqlConnection dbConnection=null;
            List<FileContent> fileNameList = new List<FileContent>();
            int control=0;
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-cs")
                {
                    control++;
                    try
                    {
                        builder = new SqlConnectionStringBuilder(args[i + 1]);
                        var ConnectionString = @"Data Source=" + builder.DataSource + ";Integrated Security=" + builder.IntegratedSecurity;
                        cnn = new SqlConnection(ConnectionString);
                        dbConnection = new SqlConnection(builder.ConnectionString);
                        DbController controls = new DbController(cnn, dbConnection);
                        var dbname = builder.InitialCatalog;
                        Console.WriteLine(dbname);
                        controls.CheckDbExists(dbname);
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("Connection string is not in a correct format", e);
                    }
                    i++;
                }
                 if (args[i] == "-p")
                {
                    control ++;
                    try
                    {
                        string[] FileNames = Directory.GetFiles(args[i + 1], "*.txt");
                        int nameCounter = 0;
                        Console.WriteLine("--- Files: ---");
                        foreach (string name in FileNames)
                        {
                            FileContent content = new FileContent();
                            content.FilePath = name;
                            content.FileName = Path.GetFileName(name);
                            fileNameList.Add(content);
                            nameCounter++;
                            Console.WriteLine("returns('{0}') File Number '{1}'", Path.GetFileName(name), nameCounter);
                        }
                    }
                    catch (ArgumentException e)
                    {
                        Console.WriteLine("File path doesn't correct!", e);
                    }
                    i++;
                }
            }
            if (control % 2 == 0)
            {
                int reader;
                reader = Int32.Parse(Console.ReadLine());
                if (reader >= 1 && reader <= 9)
                {
                    string filecontent = System.IO.File.ReadAllText(fileNameList.ElementAt(reader - 1).FilePath);
                    VersionSpaces version = new VersionSpaces(dbConnection);
                    version.CreateVersion(filecontent, fileNameList.ElementAt(reader - 1).FileName);
                }
                else
                {
                    Console.WriteLine("You must enter defined number \n ");
                }
            }
            else
            {
                Console.WriteLine("Argument has not contain ''- cs or -p '' paramater.");
            }                   
       }
    }
}
