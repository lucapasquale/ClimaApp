using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace ClimaApp.Silo
{
    public class SiloDevice : DeviceModel<SiloRX>
    {
        public SiloVentConfig ventConfig { get; set; }
        public SiloConfig siloConfig { get; set; }

        public override async Task GetData()
        {
            await base.GetData();

            //Se não existe no database insere, se existe atualiza
            var db = new Common.Database.SiloDb();
            foreach (SiloRX rx in dados)
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

    public class SiloConfig
    {
        public string grao { get; set; }
        public float altura { get; set; }
        public float diametro { get; set; }

        public SiloConfig(string _grao, float _altura, float _diametro)
        {
            grao = _grao;
            altura = _altura;
            diametro = _diametro;
        }
    }

    public class SiloVentConfig
    {
        public string configName;

        public int difUmidade;

        public SiloVentConfig(string _name, int _umid)
        {
            configName = _name;
            difUmidade = _umid;
        }
    }
}
