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
            var sw = Stopwatch.StartNew();
            await base.GetData();
            //var dadosServer = dados;
            //var db = new Common.Database.ClimaDb();
            //var DadosDB = db.GetDadosDevice(lora.deveui);

            //for (int i = 0; i < dadosServer.Count; i++)
            //{
            //    if (dadosServer[i].horario > DadosDB[0].horario)
            //        dados.Add
            //    else
            //        break;
            //}

            Debug.WriteLine("Tempo para pegar dados do servidor: " + sw.ElapsedMilliseconds);
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

        public void SaveOnDb()
        {
            var sw = Stopwatch.StartNew();
           
            //Se não existe no database insere, se existe atualiza
            var db = new Common.Database.ClimaDb();
            for (int i = 0; i < dados.Count; i++)
            {
                db.conexaoSQLite.InsertOrIgnore(dados[i]);
            }

            Debug.WriteLine("Tempo para salvar no DB: " + sw.ElapsedMilliseconds);
        }
    }
}
