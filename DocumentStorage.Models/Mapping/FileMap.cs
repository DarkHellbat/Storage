using DocumentStorage.Models.Models;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentStorage.Models.Mapping
{
    public class FileMap : ClassMap<File>
    {
        public FileMap()
        {
            Id(f => f.Id).GeneratedBy.Identity();
            Map(f => f.Name).Length(1000);
          //  Map(f => f.Content).Length(int.MaxValue);
            Map(f => f.Type).Length(50);
            Map(f => f.CreationDate);
            References(f => f.Author);
            Map(f => f.Path).Length(int.MaxValue);

        }
    }
}

