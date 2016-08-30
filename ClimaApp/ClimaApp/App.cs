using System;
using System.Collections.Generic;
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
            if (new DevicesDb().GetDevices().Count == 0)
                await DataResources.GetNodes();

            foreach (ClimaDevModel cDev in DataResources.climaNodes)
                await cDev.GetLatest();

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
            return new NavigationPage(new Pages.Clima.NodesClima());
        }
    }
}
