using Microsoft.AspNet.Identity;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace DocumentStorage.Repository
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(ISession session) :
            base(session)
        {
        }

        //public override void SetupFilter(UserFilter filter, ICriteria crit)
        //{
        //    base.SetupFilter(filter, crit);
        //    if (filter != null)
        //    {
        //        if (!string.IsNullOrEmpty(filter.UserName))
        //        {
        //            crit.Add(Restrictions.Eq("UserName", filter.UserName));
        //        }
        //        if (!string.IsNullOrEmpty(filter.FIO))
        //        {
        //            crit.Add(Restrictions.Like("FIO", filter.FIO, MatchMode.Anywhere));
        //        }

        //    }
        //}

        public User FindByLogin(string login)
        {
            var crit = session.CreateCriteria<User>();
            crit.Add(Restrictions.Eq("Login", login));
            try
            {
                return crit.List<User>().FirstOrDefault();
            }
            catch
            {
                User user = null;
                return user;
            }
        }

        //public IList<User> Find(UserFilter filter, FetchOptions options = null)
        //{
        //    var crit = session.CreateCriteria<User>();
        //    SetupFilter(filter, crit);
        //    SetupFetchOptions(crit, options);
        //    return crit.List<User>();
        //}

        //public long Count(UserFilter filter)
        //{
        //    var crit = session.CreateCriteria<User>();
        //    SetupFilter(filter, crit);
        //    crit.SetProjection(Projections.Count("Id"));
        //    return Convert.ToInt64(crit.UniqueResult());
        //}

        public User GetCurrentUser(IPrincipal user = null)
        {
            user = user ?? (HttpContext.Current != null ? HttpContext.Current.User : Thread.CurrentPrincipal);
            if (user == null || user.Identity == null)
            {
                return null;
            }
            var currentUserId = user.Identity.GetUserId();
            long userId;
            if (string.IsNullOrEmpty(currentUserId) || !long.TryParse(currentUserId, out userId))
            {
                return null;
            }
            return session.Get<User>(userId);
        }

    }
}
