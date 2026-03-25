using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNet.Membership.OpenAuth;

namespace FIlms
{
    /// <summary>
    /// Authentication configuration
    /// </summary>
    public static class AuthConfig
    {
        /// <summary>
        /// Register authentication providers
        /// </summary>
        public static void RegisterOpenAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OpenAuth.AuthenticationClients.AddTwitter(
            //    consumerKey: "your Twitter consumer key",
            //    consumerSecret: "your Twitter consumer secret");

            //OpenAuth.AuthenticationClients.AddFacebook(
            //    appId: "your Facebook app id",
            //    appSecret: "your Facebook app secret");

            //OpenAuth.AuthenticationClients.AddMicrosoft(
            //    clientId: "your Microsoft app client id",
            //    clientSecret: "your Microsoft app client secret");

            //OpenAuth.AuthenticationClients.AddGoogle();
        }
    }
}
