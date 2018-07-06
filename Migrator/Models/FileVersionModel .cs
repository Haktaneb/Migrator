using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Migrator.Models
{
    class FileVersionModel
    {
        public String Path { get; set; }
        public String Name { get; set; }
        public int Version { get; set; }
        public bool IsUp { get; set; }
    }
}
