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
            for (int i =0; i <= dados.Count; i++)
            {
                if (db.GetDado(dados[i].Id) == null)
                    db.InserirDado(dados[i]);
                else
                {
                    Debug.WriteLine("Dados salvos no database");
                    return;
                }   
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
    }
}
