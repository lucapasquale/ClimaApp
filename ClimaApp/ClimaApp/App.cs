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
            MainPage = GetMainPage();
        }

        protected override void OnStart()
        {
    
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
            return new NavigationPage(new Pages.LoginPage());
        }
    }
}
