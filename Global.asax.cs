using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Threading;
using System.Security.Principal;
using InternalProject.Services;
using System.Web.Helpers;

namespace InternalProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            /*
             * Following line supress checking for antiforgery key.
             * TODO: We will keep it during the development for it should be removed for final product.
             */
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;


            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings
            .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


        }

        internal protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // encrypt sensitive info
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                EncryptionHelper encryptionHelper = new EncryptionHelper();
                encryptionHelper.EncryptStrings("connectionStrings");
                encryptionHelper.EncryptStrings("appSettings");
            }
        }

        void Application_PostAuthenticateRequest()
        {
            if (User.Identity.IsAuthenticated)
            {
                var name = User.Identity.Name; // Get current user name.

                DB_110727_binarybaseEntities context = new DB_110727_binarybaseEntities();
                var user = context.AspNetUsers.Where(u => u.UserName == name).FirstOrDefault();
                IQueryable<string> roleQuery = from u in context.AspNetUsers
                                               from r in u.AspNetRoles
                                               where u.UserName == Context.User.Identity.Name
                                               select r.Name;
                string[] roles = roleQuery.ToArray();

                HttpContext.Current.User = Thread.CurrentPrincipal =
                                           new GenericPrincipal(User.Identity, roles);
            }
        }
    }
}
