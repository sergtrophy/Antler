﻿using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Blog.Web.Common;
using Blog.Web.Common.AppStart;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Domain.Configuration;

using SmartElk.Antler.NHibernate.SqlServer;
using SmartElk.Antler.Windsor;

namespace Blog.Web.NH.SqlServer
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IAntlerConfigurator AntlerConfigurator { get; private set; }

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new BlogViewEngine());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            
            AntlerConfigurator = new AntlerConfigurator();
            AntlerConfigurator.UseWindsorContainer()
                              .UseStorage(NHibernatePlusSqlServer.Use("Data Source=.\\SQLEXPRESS;Initial Catalog=Antler;Integrated Security=True")
                                                                  .WithMappings(Assembly.Load("Blog.Mappings.NH")).GenerateDatabase(true));
                        
            AntlerConfigurator.CreateInitialData();            
        }
        
        protected void Application_End()
        {
            if (AntlerConfigurator != null)
                AntlerConfigurator.Dispose();
        }
    }
}