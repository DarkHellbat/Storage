using Autofac;
using Autofac.Integration.Mvc;
using DocumentStorage.Controllers;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

[assembly: OwinStartup(typeof(DocumentStorage.Startup))]
namespace DocumentStorage
{

    public partial class Startup
    {
        public static void Configuration(IAppBuilder app)
        {
            var connectionSttring = ConfigurationManager.ConnectionStrings["MSSQL"];
            if (connectionSttring == null)
            { throw new Exception("not found"); }
            var builder = new ContainerBuilder();
            builder.Register(x =>
            {
                var cfg = Fluently.Configure()
                                .Database(MsSqlConfiguration.MsSql2012
                                .ConnectionString(connectionSttring.ConnectionString))
                                .Mappings( m => { m.FluentMappings.AddFromAssemblyOf<User>();
                                    //m.HbmMappings.AddFromAssemblyOf<User>(); -- раскомментить если работа через nhm.xml
                                })
                                .CurrentSessionContext("call");
                var schemaExport = new SchemaUpdate(cfg.BuildConfiguration());
                schemaExport.Execute(true, true);
                return cfg.BuildSessionFactory();
            }).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .InstancePerRequest();
            builder.RegisterControllers(Assembly.GetAssembly(typeof(AccountController)));
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterGeneric(typeof(Repository.Repository<>));
            //builder.RegisterType(typeof(Repository.UserRepository));
           builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Repository.UserRepository)));
            //builder.RegisterFilterProvider();
            

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            app.UseAutofacMiddleware(container);

            app.CreatePerOwinContext(() => new UserManager(new IdentityStore(DependencyResolver.Current.GetServices<ISession>().FirstOrDefault())));
            app.CreatePerOwinContext<ApplicationSignInManager>((options, context) => new ApplicationSignInManager(context.GetUserManager<UserManager>(), context.Authentication));

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider()
            });
        }
    }
}