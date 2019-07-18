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
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

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
                                .Mappings( m => {
                                    m.HbmMappings.AddFromAssemblyOf<User>();// -- раскомментить если работа через nhm.xml
                                    m.FluentMappings.AddFromAssemblyOf<User>();
                                  //  m.HbmMappings.AddFromAssembly("DocumentStorage.Models");
                                   
                                })
                                .CurrentSessionContext("call");
                var schemaExport = new SchemaUpdate(cfg.BuildConfiguration());
                schemaExport.Execute(true, true);
                return cfg.BuildSessionFactory();
            }).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .InstancePerLifetimeScope();
            builder.RegisterControllers(Assembly.GetAssembly(typeof(AccountController)));
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterGeneric(typeof(Repository.Repository<>));
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(Repository.UserRepository)));
            var container = builder.Build().BeginLifetimeScope();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
             //ContainerBuilder ApplicationContainer = container;
            app.UseAutofacMiddleware(container);
             try
            {
                var CreateProcedure = @"CREATE PROCEDURE [dbo].[sp_InsertFile] @Name nvarchar(100), @Type nvarchar(50), @CreationDate DateTime2, @Author_id bigint, @Path nvarchar(255) AS INSERT INTO [File] (Name, Type, CreationDate, Author_id, Path ) VALUES ( @Name, @Type, @CreationDate, @Author_id, @Path) SELECT SCOPE_IDENTITY() ";

                var result = container.Resolve<ISession>().CreateSQLQuery(CreateProcedure);//container.Resolve<ISession>()
                var a = result;
                result.ExecuteUpdate();

            }
            catch (Exception ex)
            { }
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