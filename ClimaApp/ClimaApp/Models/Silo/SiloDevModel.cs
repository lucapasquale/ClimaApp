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
    public class SiloDevModel : Models.Base.DeviceModel<SiloRxModel>
    {
        public override async Task GetData()
        {
            await base.GetData();

            //Se não existe no database insere, se existe atualiza
            var db = new Common.Database.SiloDb();
            foreach (SiloRxModel rx in dados)
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
                //Se não existe dados no servidor, tentar pegar o ultimo do DB
                var db = new Common.Database.SiloDb();
                if (db.GetDadosDevice(lora.deveui).Count > 0)
                    latest = db.GetDadosDevice(lora.deveui)[0];
            }
        }
    }
}
