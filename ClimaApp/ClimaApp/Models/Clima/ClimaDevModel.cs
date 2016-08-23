using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ClimaApp
{
    public class ClimaDevModel : DeviceModel
    {
        public ObservableCollection<ClimaRxModel> dados = new ObservableCollection<ClimaRxModel>();
        public ClimaRxModel latest = new ClimaRxModel();


        public async Task PegarDados(string _devEUI)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + _devEUI + "/payloads/ul");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<List<ClimaRxModel>>(request);

            var listaTemp = new ObservableCollection<ClimaRxModel>();
            foreach (ClimaRxModel rx in result.Data)
            {
                //Pega o horario em DateTime
                rx.horario = DateTime.Parse(rx.timeStamp);
                TimeZoneInfo.ConvertTime(rx.horario, TimeZoneInfo.Local);

                //Passa de base64 para HEX e remove os '-' entre os bytes
                byte[] data = Convert.FromBase64String(rx.dataFrame);
                rx.dataFrame = BitConverter.ToString(data).Replace("-", string.Empty);

                //Transforma de HEX para as variaveis de cada aplicação
                rx.ParseDataFrame();

                listaTemp.Add(rx);
            }
            listaTemp = new ObservableCollection<ClimaRxModel>(listaTemp.OrderByDescending(o => o.horario));

            dados.Clear();
            dados = listaTemp;
        }

        public async Task PegarLatest(string _devEUI)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + _devEUI + "/payloads/ul/latest");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<ClimaRxModel>(request);

            var rx = result.Data;

            //Pega o horario em DateTime
            rx.horario = DateTime.Parse(rx.timeStamp);
            TimeZoneInfo.ConvertTime(rx.horario, TimeZoneInfo.Local);

            //Passa de base64 para HEX e remove os '-' entre os bytes
            byte[] data = Convert.FromBase64String(rx.dataFrame);
            rx.dataFrame = BitConverter.ToString(data).Replace("-", string.Empty);

            //Transforma de HEX para as variaveis de cada aplicação
            rx.ParseDataFrame();

            latest = rx;
        }
    }
}
