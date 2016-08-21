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
    public class DeviceModel
    {
        public NodeModel loraNode = new NodeModel();
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

                //Converte de HEX para decimal
                rx.temperatura = int.Parse(rx.dataFrame.Substring(0, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
                rx.umidade = int.Parse(rx.dataFrame.Substring(4, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
                rx.pressao = int.Parse(rx.dataFrame.Substring(8, 4), System.Globalization.NumberStyles.HexNumber);

                listaTemp.Add(rx);
            }
            listaTemp = new ObservableCollection<ClimaRxModel>(listaTemp.OrderByDescending(o => o.horario));

            dados.Clear();
            for (int i = 0; i < listaTemp.Count; i++)
                dados.Add(listaTemp[i]);
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

            //Converte de HEX para decimal
            rx.temperatura = int.Parse(rx.dataFrame.Substring(0, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
            rx.umidade = int.Parse(rx.dataFrame.Substring(4, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
            rx.pressao = int.Parse(rx.dataFrame.Substring(8, 4), System.Globalization.NumberStyles.HexNumber);

            latest = rx;
        }
    }
}
