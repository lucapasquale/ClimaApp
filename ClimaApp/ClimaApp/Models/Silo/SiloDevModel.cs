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
    public class SiloDevModel : DeviceModel
    {
        public ObservableCollection<SiloRxModel> todosDados = new ObservableCollection<SiloRxModel>();
        public SiloRxModel latest = new SiloRxModel();

        public ObservableCollection<SiloRxModel>[] dadosSilos = new ObservableCollection<SiloRxModel>[8];


        public async Task PegarDados(string _devEUI)
        {
            var client = new RestClient();
            client.BaseUrl = new Uri("https://artimar.orbiwise.com/rest/nodes/" + _devEUI + "/payloads/ul");
            client.Authenticator = new HttpBasicAuthenticator(StringResources.user, StringResources.pass);

            var request = new RestRequest();
            var result = await client.Execute<List<SiloRxModel>>(request);

            var listaTemp = new ObservableCollection<SiloRxModel>[8];
            bool[] siloInstanciado = new bool[8];
            foreach (SiloRxModel rx in result.Data)
            {
                //Pega o horario em DateTime
                rx.horario = DateTime.Parse(rx.timeStamp);
                TimeZoneInfo.ConvertTime(rx.horario, TimeZoneInfo.Local);

                //Passa de base64 para HEX e remove os '-' entre os bytes
                byte[] data = Convert.FromBase64String(rx.dataFrame);
                rx.dataFrame = BitConverter.ToString(data).Replace("-", string.Empty);

                //Transforma de HEX para as variaveis de cada aplicação
                rx.ParseDataFrame();

                if (!siloInstanciado[rx.idSilo])
                {
                    listaTemp[rx.idSilo] = new ObservableCollection<SiloRxModel>();
                    siloInstanciado[rx.idSilo] = true;
                }
                listaTemp[rx.idSilo].Add(rx);
            }

            for (int i = 0; i < listaTemp.Length; i++)
            {
                if (listaTemp[i].Count > 0)
                {
                    listaTemp[i] = new ObservableCollection<SiloRxModel>(listaTemp[i].OrderByDescending(o => o.horario));

                    dadosSilos[i] = new ObservableCollection<SiloRxModel>();
                    dadosSilos[i] = listaTemp[i];
                }
            }
        }
    }
}
