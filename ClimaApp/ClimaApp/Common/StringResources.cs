using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    static class StringResources
    {
        public static bool isLoggedIn = false;

        public static string user = "maua_ceun";

        public static RestClient restClient = new RestClient()
        {
            BaseUrl = new Uri("https://artimar.orbiwise.com/rest"),
            Authenticator = new HttpBasicAuthenticator("maua_ceun", "Maua2016"),
        };
    }
}
