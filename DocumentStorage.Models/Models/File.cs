using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentStorage.Models.Models
{
   public class File
    {
        public virtual long Id { get; set; }
       // public virtual byte[] Content { get; set; }
        public virtual string Name { get; set; }
        public virtual string Type { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual User Author { get; set; }
        public virtual string Path { get; set; }
    }
}
