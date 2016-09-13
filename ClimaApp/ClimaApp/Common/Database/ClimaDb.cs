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
    class ClimaDb : DadosBaseDb<ClimaRxModel>, IDisposable
    {
        public ClimaDb()
        {
            var config = DependencyService.Get<IConfig>();
            conexaoSQLite = new SQLiteConnection(config.plataforma, Path.Combine(config.diretorioSQLite, StringResources.user + "-clima.db3"));
            conexaoSQLite.CreateTable<ClimaRxModel>();
        }
    }
}