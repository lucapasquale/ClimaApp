using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class LoRaModel
    {
        [PrimaryKey]
        public string deveui { get; set; }
        public string last_reception { get; set; }
        public string appeui { get; set; }
        public string comment { get; set; }

        public AppType tipo { get; set; }
        public DateTime horaUltimoRx { get; set; }


        public async Task GetTipo()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + deveui + "/payloads/ul/latest");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<RxModel>(request);

            if (!String.IsNullOrEmpty(result.Content))
            {
                horaUltimoRx = DateTime.Parse(last_reception);
                TimeZoneInfo.ConvertTime(horaUltimoRx, TimeZoneInfo.Local);

                switch (result.Data.port)
                {
                    case (int)AppType.Clima: tipo = AppType.Clima; break;   //Port = 10
                    case (int)AppType.Testes: tipo = AppType.Silo; break;   //Port = 30
                    default: tipo = AppType.Testes; break;
                }
            }
            else
                tipo = AppType.None;
        }
    }
}
