﻿
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;
using Collision.Sql.Ef.Services;
using Collision.Sql.Ef.Services.Interfaces;
using Collision.v1.API;

namespace Collision
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

            //GlobalConfiguration.Configuration.DependencyResolver = new Unity.WebApi.UnityDependencyResolver(container);
            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            container.RegisterType<IConflictService, ConflictService>();
            container.RegisterType<ConflictController>();


            return container;
        }
    }
}
