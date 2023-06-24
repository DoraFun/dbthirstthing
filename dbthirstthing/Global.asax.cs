﻿using dbthirstthing.Interfaces;
using dbthirstthing.Jobs;
using dbthirstthing.Models;
using dbthirstthing.Services;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using hbehr.recaptcha;

using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Unity;
using Unity.AspNet.Mvc;

namespace dbthirstthing
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //ReCaptcha init
            string publicKey = "6Ld1B3UmAAAAANEOmK1ePUBzhmEM6ovwThPAVXqu";
            string secretKey = "6Ld1B3UmAAAAAOrJiYXFKZA9mLbhw0StDpt1AiZR";

            // Optional, select a default language:
            ReCaptchaLanguage defaultLanguage = ReCaptchaLanguage.Russian;
            ReCaptcha.Configure(publicKey, secretKey, defaultLanguage);

            //ILogger init
            ILogger logger = LogManager.GetCurrentClassLogger();
            logger.Trace("Application started");

            //Background server load monitoring
            MonitorScheduler.Start();


            //AutoMapper config
            AutoMapperConfig.RegisterMappings();

            var container = new UnityContainer();
            container.RegisterType<INewsService, NewsService>();
            container.RegisterType<IPreregistrationService, PreregistrationService>();
            container.RegisterType<IUserService, UserService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            
        }
    }
}
