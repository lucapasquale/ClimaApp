using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ClimaApp
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
            client.Authenticator = StringResources.auth;

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
            latest = dados.Count > 0 ? dados[0] : null;
        }

        public virtual async Task GetLatest()
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + lora.deveui + "/payloads/ul/latest");
            client.Authenticator = StringResources.auth;

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

        public virtual async Task SendData(string _data, int _port = 30)
        {
            byte[] dataBytes = Encoding.UTF8.GetBytes(_data);
            string data = Convert.ToBase64String(dataBytes);

            var client = new RestClient();
            string url = string.Format("https://artimar.orbiwise.com/rest/nodes/{0}/payloads/dl?port={1}", lora.deveui, _port);
            client.BaseUrl = new Uri(url);
            client.Authenticator = StringResources.auth;

            var request = new RestRequest(Method.POST);
            request.AddParameter("text/plain", /*data*/ string.Empty, ParameterType.RequestBody);



            var body = request.Parameters.Where(p => p.Type == ParameterType.RequestBody).FirstOrDefault();
            if (body != null)
            {
                Debug.WriteLine("Current Body = {0} / {1}", body.ContentType, body.Value);
            }

            try
            {
                var result = await client.Execute(request);
                Debug.WriteLine(result.Content);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                Debug.WriteLine(e.Message);

            }
        }
    }
}
