using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp.Common.Database
{
    class ClimaDb : IDisposable
    {
        private SQLiteConnection conexaoSQLite;

        public ClimaDb()
        {
            var config = DependencyService.Get<IConfig>();
            conexaoSQLite = new SQLiteConnection(config.plataforma, Path.Combine(config.diretorioSQLite, "Clima.db3"));
            conexaoSQLite.CreateTable<ClimaRxModel>();
        }

        public void InserirDado(ClimaRxModel dado)
        {
            conexaoSQLite.Insert(dado);
        }

        public void AtualizarDado(ClimaRxModel dado)
        {
            conexaoSQLite.Update(dado);
        }

        public void DeletarModulo(ClimaRxModel dado)
        {
            conexaoSQLite.Delete(dado);
        }

        public ClimaRxModel GetDado(long _Id)
        {
            return conexaoSQLite.Table<ClimaRxModel>().FirstOrDefault(c => c.Id == _Id);
        }

        public ClimaRxModel GetDadoHora(DateTime _horario)
        {
            return conexaoSQLite.Table<ClimaRxModel>().FirstOrDefault(c => c.horario == _horario);
        }

        public List<ClimaRxModel> GetDados(string _devEUI)
        {
            List<ClimaRxModel> temp = new List<ClimaRxModel>();
            foreach (ClimaRxModel rx in conexaoSQLite.Table<ClimaRxModel>())
            {
                if (rx.devEUI == _devEUI)
                    temp.Add(rx);
            }
            return temp.OrderByDescending(c => c.timeStamp).ToList();
        }

        public void Dispose()
        {
            conexaoSQLite.Dispose();
        }
    }
}