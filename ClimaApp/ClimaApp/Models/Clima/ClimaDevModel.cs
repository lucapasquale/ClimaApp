using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class ClimaDevModel : Models.Base.DeviceModel<ClimaRxModel>
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
    }
}
