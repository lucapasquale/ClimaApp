using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Threading.Tasks;

namespace ClimaApp.Models.Base
{
    public class DeviceModel<T> where T : RxModel, new()
    {
        public LoRaModel lora { get; set; }
        public List<T> dados { get; set; }
        public T latest { get; protected set; }

        public virtual async Task GetData()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + lora.deveui + "/payloads/ul");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<List<T>>(request);

            var listaTemp = new List<T>();
            foreach (T rx in result.Data)
            {
                //Grava o devEUI para guardar no database
                rx.devEUI = lora.deveui;

                //Pega o horario em DateTime
                rx.horario = DateTime.Parse(rx.timeStamp);
                TimeZoneInfo.ConvertTime(rx.horario, TimeZoneInfo.Local);

                //Se o node esta online, atrasado ou offline
                lora.GetStatus();

                //Passa de base64 para HEX e remove os '-' entre os bytes
                byte[] data = Convert.FromBase64String(rx.dataFrame);
                rx.dataFrame = BitConverter.ToString(data).Replace("-", string.Empty);

                //Transforma de HEX para as variaveis de cada aplicação
                rx.ParseDataFrame();

                listaTemp.Add(rx);
            }
            dados = listaTemp.OrderByDescending(o => o.horario).ToList();
        }

        public virtual async Task GetLatest()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + lora.deveui + "/payloads/ul/latest");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<T>(request);

            if (string.IsNullOrEmpty(result.Content))
            {
                latest = null;
                return;
            }
            else
            {
                var rx = result.Data;

                //Grava o devEUI para guardar no database
                rx.devEUI = lora.deveui;

                //Pega o horario em DateTime
                rx.horario = DateTime.Parse(rx.timeStamp);
                TimeZoneInfo.ConvertTime(rx.horario, TimeZoneInfo.Local);

                //Se o node esta online, atrasado ou offline
                lora.GetStatus();

                //Passa de base64 para HEX e remove os '-' entre os bytes
                byte[] data = Convert.FromBase64String(rx.dataFrame);
                rx.dataFrame = BitConverter.ToString(data).Replace("-", string.Empty);

                //Transforma de HEX para as variaveis de cada aplicação
                rx.ParseDataFrame();

                latest = rx;
            }

        }
    }
}
