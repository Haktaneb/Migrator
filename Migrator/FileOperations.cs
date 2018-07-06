using Migrator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator
{
    class FileOperations
    {
        //private readonly string fileDirectoryPath;
        private string[] scriptFiles;
        private readonly List<FileVersionModel> fileVersions = new List<FileVersionModel>();


        public void FileReader(string fileDirectoryPath)
        {
            scriptFiles = Directory.GetFiles(fileDirectoryPath, "*.sql");
            FileSeparator();          
        }

        public void FileSeparator()
        {
            foreach (var file in scriptFiles)
            {
                FileVersionModel fileContent = new FileVersionModel();
                fileContent.Path = file;
                fileContent.Name = Path.GetFileNameWithoutExtension(file);
                fileContent.Version = GetVersionNumberFromVersionName(fileContent.Name);
                fileContent.IsUp = GetUpOrDown(fileContent.Name);
                fileVersions.Add(fileContent);
            }           
        }
        public List<FileVersionModel> GetFileVersions()
        {
            return fileVersions;
        }
        public int GetFileCurrentVersion()
        {
            var FileVersion = scriptFiles[scriptFiles.Length - 1];
            return GetVersionNumberFromVersionName(Path.GetFileNameWithoutExtension(FileVersion));
        }

        public int GetVersionNumberFromVersionName(string fileName)
        {
            int index = fileName.IndexOf('_');
            var versionStr = index == -1 ? fileName : fileName.Substring(0, index).Remove(0, 1);
            return Convert.ToInt32(versionStr);
        }
        public bool GetUpOrDown(string fileName)
        {
            bool controller = fileName.Contains("UP");
            return controller;
        }
    }
}
