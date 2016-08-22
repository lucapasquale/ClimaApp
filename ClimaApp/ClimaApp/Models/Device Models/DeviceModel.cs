using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Collections.ObjectModel;

namespace ClimaApp
{
    public enum AppType {Testes, Clima, Silo};

    public class DeviceModel
    {
        public string deveui { get; set; }
        public string last_reception { get; set; }
        public string appeui { get; set; }
        public string comment { get; set; }

        public AppType tipo { get; private set; }
        public DateTime ultimoTx { get; private set; }

        public async Task PegarTipo()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + deveui + "/payloads/ul/latest");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<RxModel>(request);

            if (!String.IsNullOrEmpty(result.Content))
            {
                switch (result.Data.port)
                {
                    case 10: tipo = AppType.Clima; break;
                    case 30: tipo = AppType.Silo; break;
                    default: tipo = AppType.Testes; break;
                }
            }
            else
                tipo = AppType.Testes;
        }


        public static async Task PegarNodes()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<ObservableCollection<DeviceModel>>(request);
            var listaTemp = result.Data;

            foreach (DeviceModel node in listaTemp)
            {
                await node.PegarTipo();
                node.ultimoTx = DateTime.Parse(node.last_reception);
                TimeZoneInfo.ConvertTime(node.ultimoTx, TimeZoneInfo.Local);
            }
                
            DataResources.nodes.Clear();
            for (int i = 0; i < listaTemp.Count; i++)
                DataResources.nodes.Add(listaTemp[i]);
        }
    }
}
