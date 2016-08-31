using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp
{
    public class DevicesDb : IDisposable
    {
        private SQLiteConnection conexaoSQLite;

        public DevicesDb()
        {
            var config = DependencyService.Get<IConfig>();
            conexaoSQLite = new SQLiteConnection(config.plataforma, Path.Combine(config.diretorioSQLite, "LoRa.db3"));
            conexaoSQLite.CreateTable<LoRaModel>();
        }
        public void InserirModulo(LoRaModel cliente)
        {
            conexaoSQLite.Insert(cliente);
        }
        public void AtualizarModulo(LoRaModel cliente)
        {
            conexaoSQLite.Update(cliente);
        }
        public void DeletarModulo(LoRaModel cliente)
        {
            conexaoSQLite.Delete(cliente);
        }
        public LoRaModel GetModulo(string codigo)
        {
            return conexaoSQLite.Table<LoRaModel>().FirstOrDefault(c => c.deveui == codigo);
        }
        public List<LoRaModel> GetModulos()
        {
            return conexaoSQLite.Table<LoRaModel>().OrderBy(c => c.comment).ToList();
        }
        public void Dispose()
        {
            conexaoSQLite.Dispose();
        }
    }
}
