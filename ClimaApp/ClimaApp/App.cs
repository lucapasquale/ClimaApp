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

            //DataResources.climaNodes.Add(new ClimaDevModel() { lora = new LoRaModel() { comment = "Balizador Mauá", status = NodeStatus.Online }, dados =  GeraDados()});
            DataResources.nivelNodes.Add(new Nivel.NivelDevice("Caixa d'água Bloco H", 1.2f));
            DataResources.nivelNodes.Add(new Nivel.NivelDevice("Caixa d'água Instituto Mauá", 2.3f));
        }

        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected async override void OnResume()
        {
            if (StringResources.isLoggedIn)
            {
                try
                {
                    await DataResources.GetNodes();
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                }
            }
        }

        public static Page GetMainPage()
        {
            return new NavigationPage(new Pages.LoginPage());
        }

        //public List<ClimaRxModel> GeraDados()
        //{
        //    float[] temps = { 25, 25, 25, 24.7f, 24, 24.7f, 24.1f, 23.7f, 23.2f, 23.3f, 23.7f, 23.9f, 24f,
        //        23.6f, 23.3f, 23.1f, 22.9f, 22.8f, 22.6f, 22.6f, 22.4f, 22.3f, 22.2f, 22.1f, 22f, 21.9f, 21.8f,
        //        21.7f, 21.5f, 21.4f, 21.4f, 21.3f, 21.2f, 21.1f, 21f, 20.9f, 20.8f, 20.9f, 20.8f, 20.8f, 20.8f,
        //        20.8f, 20.8f, 20.7f, 20.7f, 20.7f, 20.8f, 20.8f, 20.9f, 21.3f, 21f, 20.7f, 20.6f, 20.5f, 20.5f,
        //        20.6f, 20.6f, 20.8f, 20.8f, 20.8f, 21f, 21.1f, 21.5f, 21.9f, 22.2f, 22.2f, 22f };

        //    float[] umid = { 56.3f, 56.7f, 56.9f, 57.8f, 61.4f, 64f, 65.1f, 63.1f, 62.5f, 57.7f, 57.1f, 56.3f,
        //        55.5f, 55.4f, 55.4f, 55.8f, 55.6f, 56.2f, 56.1f, 56.1f, 56.6f, 56.9f, 57f, 56.9f, 56.8f, 55.7f,
        //        54.6f, 53.7f, 54.2f, 55f, 55.5f, 56f, 55.4f, 54.4f, 54.3f, 54.9f, 55f, 54.7f, 54.9f, 55.2f, 54.7f,
        //        56f, 56.7f, 56.7f, 57f, 57.2f, 59.4f, 58.1f, 59.5f, 61.2f, 60.9f, 61.4f, 61.4f, 61.9f, 62.4f,
        //        62.6f, 62.7f, 61.7f, 60.7f, 46.5f, 46.8f, 43.5f, 44f, 54.6f, 56.1f, 56.6f, 56.7f, 56.5f, 56.3f,
        //        56.1f, 55.9f, 55.5f, 54.8f, 54.8f, 54.4f, 54.1f, 53.6f, 53.6f, 53.6f, 53.5f, 53.3f };

        //    int[] pres = { 0922, 922, 922, 922, 922, 922, 922, 922, 923, 923, 923, 923, 923, 923, 923, 923,
        //        923, 923, 923, 923, 922, 922, 923, 922, 923, 923, 923, 923, 923, 923, 924, 923, 924, 924, 923,
        //        924, 924, 924, 924, 925, 925, 924, 925, 925, 925, 925, 925, 925, 924, 924, 924, 924, 924, 924,
        //        924, 923, 923, 923, 922, 922, 922, 922, 922, 922, 922, 922, 922, 923, 923, 923, 924, 924, 924,
        //        923, 924, 924, 925, 925, 925, 925, 925 };

        //    var rng = new Random();
        //    var temp = new List<ClimaRxModel>();

        //    for (int j = 0; j < 24 * 14; j++)
        //        temp.Add(new ClimaRxModel() { horario = DateTime.Now.AddMinutes(-j * 15),
        //            temperatura = temps[j % temps.Count()],
        //            umidade = umid[j % umid.Count()],
        //            pressao = pres[j % pres.Count()]});

        //    return temp;
        //}
    }
}
