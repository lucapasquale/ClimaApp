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
        public LoRaModel lora { get; set; } = new LoRaModel();
        public List<T> dados { get; set; } = new List<T>();
        public T latest { get; set; } = new T();

        public virtual async Task GetData()
        {
            var request = new RestRequest("nodes/{devEUI}/payloads/ul");
            request.AddUrlSegment("devEUI", lora.deveui.ToUpper());

            var result = await StringResources.restClient.Execute<List<T>>(request);

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
            var request = new RestRequest("nodes/{devEUI}/payloads/ul/latest");
            request.AddUrlSegment("devEUI", lora.deveui.ToUpper());

            var result = await StringResources.restClient.Execute<T>(request);

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

        public virtual async Task SendData(byte[] _data)
        {
            string dataB64 = Convert.ToBase64String(_data);
            byte[] dataBytesB64 = Encoding.UTF8.GetBytes(dataB64);

            var request = new RestRequest("nodes/{devEUI}/payloads/dl?port={port}", Method.POST);
            request.AddUrlSegment("devEUI", lora.deveui);
            request.AddUrlSegment("port", (int)lora.tipo);

            request.AddParameter("text/plain", dataBytesB64, ParameterType.RequestBody);
            Debug.WriteLine("Sending: " + dataBytesB64);


            try
            {
                var result = await StringResources.restClient.Execute(request);
                Debug.WriteLine(result.Content);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public virtual async Task SendData(string _devEUI, byte _comando, byte[] _dado)
        {
            byte[] dado = new byte[8 + 1 + _dado.Length];

            //devEUI de string para byte[]
            byte[] dataDev = StringToByteArray(_devEUI);
            for (int i = 0; i < dataDev.Length; i++)
                dado[i] = dataDev[i];

            //Adiciona byte de comando
            dado[8] = _comando;

            //Adiciona dados adicionais
            for (int i = 0; i < _dado.Length; i++)
                dado[9 + i] = _dado[i];

            await SendData(dado);
        }

        byte[] StringToByteArray(string hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
