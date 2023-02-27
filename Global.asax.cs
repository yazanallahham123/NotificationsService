using NotificationsService.Classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.SessionState;


namespace NotificationsService
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            //Errors.LogError(1, "Log: Application_Start", "Application_Start", "1.0.3", "API", "Log: Application_Start", "Application_Start", "");
            //SqlDependency.Start(WebConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
            //FCMClass.initSQLDep();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Errors.LogError(1, "Log: Session_Start", "Session_Start", "1.0.3", "API", "Log: Session_Start", "Session_Start", "");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Errors.LogError(1, "Log: Application_BeginRequest", "Application_BeginRequest", "1.0.3", "API", "Log: Application_BeginRequest", "Application_BeginRequest", "");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, Access");
                HttpContext.Current.Response.AddHeader("Access-Control-Expose-Headers", "Access");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            //Errors.LogError(1, "Log: Application_End", "Application_End", "1.0.3", "API", "Log: Application_End", "Application_End", "");
            //SqlDependency.Stop(WebConfigurationManager.ConnectionStrings["Connection"].ConnectionString);
        }
    }
}