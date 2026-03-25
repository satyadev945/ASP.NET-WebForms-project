using System.Web.Http;
using System.Web.Security;
using FIlms;

namespace FIlms
{
    public class Global : HttpApplication
    {
            
            // Register Web API routes first
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            // Code that runs when an unhandled error occurs

        }
    }
}
