using System;

using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ClimaApp
{
    public static class DataResources
    {      
        public static List<LoRaModel> allNodes = new List<LoRaModel>();

        public static ObservableCollection<ClimaDevModel> climaNodes = new ObservableCollection<ClimaDevModel>();
        public static ClimaDevModel climaSelecionado = new ClimaDevModel();

        public static ObservableCollection<SiloDevModel> siloNodes = new ObservableCollection<SiloDevModel>();


        public static async Task GetNodes()
        {
            var db = new DevicesDb();

            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<List<LoRaModel>>(request);
            var listaTemp = result.Data;

            foreach (LoRaModel loraNode in listaTemp)
            {
                await loraNode.GetTipo();

                if (db.GetModulo(loraNode.deveui) == null)
                    db.InserirModulo(loraNode);
                else
                    db.AtualizarModulo(loraNode);
            }
            allNodes = new List<LoRaModel>(listaTemp.OrderByDescending(o => o.comment).ToList());
            SepararTipo();
        }

        public static void SepararTipo()
        {
            climaNodes.Clear();
            siloNodes.Clear();

            foreach (LoRaModel loraNode in allNodes)
            {
                switch (loraNode.tipo)
                {
                    case AppType.Clima: climaNodes.Add(new ClimaDevModel() { node = loraNode }); break;
                    case AppType.Silo: siloNodes.Add(new SiloDevModel() { node = loraNode }); break;
                    default: break;
                }
            }
        }
    }
}
