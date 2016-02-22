using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Practices.Unity;
using System.Web.Mvc;
using Microsoft.Practices.Unity.Mvc;
using Collision.Sql.Ef.Repositories;
using Collision.Sql.Ef.Repositories.Interfaces;
using Collision.Models;
using System.Data.Common;
using System.Data.Entity;
using Microsoft.Owin.Security;
using System.Web;
using Collision.Controllers;
using System.Web.Http;

namespace Collision
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<Collision.Sql.Ef.CollisionEntities>(new InjectionFactory(c => new Collision.Sql.Ef.CollisionEntities()));
            container.RegisterType<IConflictRepository, ConflictRepository>();
            //container.RegisterType<Collision.Controllers.v1.breeze.ConflictBreezeApiController>();
            //container.RegisterType<Collision.Controllers.v1.ConflictController>();

            //http://stackoverflow.com/questions/24731426/register-iauthenticationmanager-with-unity
            container.RegisterType<DbContext, ApplicationDbContext>(new HierarchicalLifetimeManager());
            container.RegisterType<UserManager<ApplicationUser>>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserStore<ApplicationUser>, UserStore<ApplicationUser>>();
            container.RegisterType<AccountController>(new InjectionConstructor());
            container.RegisterType<IAuthenticationManager>(new InjectionFactory(o => HttpContext.Current.GetOwinContext().Authentication));

            return container;
        }
    }
}
