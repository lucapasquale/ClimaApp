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
            // Handle when your app starts

            //await DeviceModel.PegarNodes();
            await DataResources.nodeTCC.PegarDados(StringResources.devEUIarduino);
            MainPage = new Pages.GraficosPage();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
