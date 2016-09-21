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
        public static int selectedIndex;

        public static List<ClimaDevModel> climaNodes = new List<ClimaDevModel>();
        public static List<Models.Silo.SiloDevice> siloNodes = new List<Models.Silo.SiloDevice>();
        public static int unnusedNodes; 


        public static async Task GetNodes()
        {
            var db = new DevicesDb();

            //Conecta ao servidor da Orbiwise e baixa os nodes da conta
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/");
            client.Authenticator = StringResources.auth;

            var request = new RestRequest();
            var result = await client.Execute<List<LoRaModel>>(request);
            var listaTemp = result.Data;

            foreach (LoRaModel loraNode in listaTemp)
            {
                loraNode.GetTipo();

                //Se não existe LoRa node no database inserir, se existe atualizar
                if (db.GetModulo(loraNode.deveui) == null)
                    db.InserirModulo(loraNode);
                else
                    db.AtualizarModulo(loraNode);
            }
            allNodes = new List<LoRaModel>(listaTemp.OrderByDescending(o => o.comment).ToList());
            SepararApplications();
        }

        public static void SepararApplications()
        {
            climaNodes.Clear();
            siloNodes.Clear();

            foreach (LoRaModel loraNode in allNodes)
            {
                switch (loraNode.tipo)
                {
                    case AppType.Clima: climaNodes.Add(new ClimaDevModel() { lora = loraNode }); break;
                    case AppType.Silo: siloNodes.Add(new Models.Silo.SiloDevice() { lora = loraNode }); break;
                    default: unnusedNodes++; break;
                }
            }
        }

        public static void ClearData()
        {
            allNodes.Clear();
            climaNodes.Clear();
            siloNodes.Clear();
            unnusedNodes = 0;
        }
    }
}
