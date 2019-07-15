using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentStorage.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(u => u.Id).GeneratedBy.Identity();
            Map(u => u.UserName).Length(30);
            Map(u => u.Password).Column("PasswordHash");
            Map(u => u.FirstName).Length(100);
            Map(u => u.LastName).Length(100);
            Map(u => u.SecondName).Length(100);
            Map(u => u.Salt).Length(30);
            Map(u => u.Email).Length(100);
            Map(u => u.DateofBirth);
           

        }
    }
}
