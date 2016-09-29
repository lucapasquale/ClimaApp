using RestSharp.Portable.Authenticators;
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

        public static HttpBasicAuthenticator auth = new HttpBasicAuthenticator("maua_ceun", "Maua2016");
    }
}
