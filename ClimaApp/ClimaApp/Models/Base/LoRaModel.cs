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
    public enum AppType { None = 0, Clima = 10, Silo = 30, Testes = 256, };
    public enum NodeStatus { OFFLINE, ATRASADO, ONLINE }

    public class LoRaModel
    {
        [PrimaryKey]
        public string deveui { get; set; }
        public string last_reception { get; set; }
        public string appeui { get; set; }
        public string comment { get; set; }

        public DateTime horaUltimoRx { get; private set; }
        public AppType tipo { get; private set; } = AppType.None;
        public NodeStatus status { get; set; } = NodeStatus.OFFLINE;

        public async Task GetTipo()
        {
            //var client = new RestClient();
            //client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + deveui + "/payloads/ul/latest");
            //client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            //var request = new RestRequest();
            //var result = await client.Execute<RxModel>(request);

            //if (!string.IsNullOrEmpty(result.Content))
            //{
            //    //Passa horario para hora local
            //    horaUltimoRx = DateTime.Parse(last_reception);
            //    TimeZoneInfo.ConvertTime(horaUltimoRx, TimeZoneInfo.Local);

            //    //Checa se o node parou de receber ou se está offline
            //    GetStatus();

            //    //Muda o tipo de Application dependendo do port recebido
            //    switch (result.Data.port)
            //    {
            //        case (int)AppType.Clima: tipo = AppType.Clima; break;   //Port = 10
            //        case (int)AppType.Silo: tipo = AppType.Silo; break;     //Port = 30
            //        default: tipo = AppType.Testes; break;
            //    }
            //}

            if (comment.Contains("Balizador"))
                tipo = AppType.Clima;
        }

        public void GetStatus()
        {
            if (horaUltimoRx.AddHours(2) > DateTime.Now)
                status = NodeStatus.ATRASADO;

            if (horaUltimoRx.AddMinutes(50) > DateTime.Now)
                status = NodeStatus.ONLINE;
        }
    }
}
