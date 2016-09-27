using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using System;
using System.Diagnostics;
using System.Linq;

namespace ClimaApp
{
    public class ClimaDevModel : DeviceModel<ClimaRxModel>
    {
        public override async Task GetData()
        {
            await base.GetData();

            //Se não existe no database insere, se existe atualiza
            var db = new Common.Database.ClimaDb();
            foreach (ClimaRxModel rx in dados)
            {
                if (db.GetDado(rx.Id) == null)
                    db.InserirDado(rx);
                else
                    db.AtualizarDado(rx);
            }
        }

        public override async Task GetLatest()
        {
            await base.GetLatest();

            if (latest == null)
            {
                var db = new Common.Database.ClimaDb();
                if (db.GetDadosDevice(lora.deveui).Count > 0)
                    latest = db.GetDadosDevice(lora.deveui)[0];
            }
        }

        public override async Task SendData(string _data, int _port = 30)
        {
            await base.SendData(_data);
        }
    }
}
