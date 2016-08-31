using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace ClimaApp
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application           
        }

        protected async override void OnStart()
        {
            DevicesDb db = new DevicesDb();

            if (db.GetModulos().Count == 0)
            {
                await DataResources.GetNodes();
                Debug.WriteLine("Nodes request" + db.GetModulos().Count.ToString());
            }
            else
            {
                DataResources.allNodes = db.GetModulos();
                DataResources.SepararTipo();
                Debug.WriteLine("DATABASE");
            }

            MainPage = GetMainPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static Page GetMainPage()
        {
            return new NavigationPage(new Pages.ApplicationsPage());
        }
    }
}
