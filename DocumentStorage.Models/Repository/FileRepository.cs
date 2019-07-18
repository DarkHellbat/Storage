using DocumentStorage.Models.Filters;
using DocumentStorage.Models.Models;
using DocumentStorage.Repository;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
//using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentStorage.Models.Repository
{
    public class FileRepository : Repository<File>
    {
        public FileRepository(ISession session) :
            base(session)
        {
        }

        public List<File> GetAll()
        {
            return session.CreateCriteria<File>().List<File>().ToList();
        }
        public IList<File> GetUsersFile(long userId, FileFilter filter, FetchOptions options)
        {
            var crit = session.CreateCriteria<File>();
            SetupFilter(crit, filter);
            crit.Add(Restrictions.Eq("Author.Id", userId));
            if (options != null)
            {
                SetFetchOptions(crit, options);
            }
            //var n = crit.List<Note>();
            return crit.List<File>();

        }
        protected virtual void SetupFilter(ICriteria crit, FileFilter filter)
        {
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.Name))
                {
                    crit.Add(Restrictions.Like("Name", filter.Name, MatchMode.Anywhere));
                }
                if (filter.CreationDate != null)
                {
                    if (filter.CreationDate!=null)
                    {
                        crit.Add(Restrictions.Ge("CreationDate", filter.CreationDate));
                    }
                    
                }
            }
        }
       public static void AddMappingsFromFilesystem(NHibernate.Cfg.Configuration nhConfiguration)
        {
            var directory = System.IO.Directory.GetCurrentDirectory();
            var hmbFiles = System.IO.Directory.GetFiles(directory, "*.hbm.xml", System.IO.SearchOption.TopDirectoryOnly);

            foreach (var file in hmbFiles)
            {
                nhConfiguration.AddFile(file);
            }
        }

    }
}
