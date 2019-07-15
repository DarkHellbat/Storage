using DocumentStorage.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentStorage.Models.Filters
{
    public class FileFilter: BaseFilter
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
