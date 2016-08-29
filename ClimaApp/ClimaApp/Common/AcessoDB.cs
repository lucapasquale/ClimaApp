using SQLite.Net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp
{
    public class AcessoDB : IDisposable
    {
        private SQLiteConnection conexaoSQLite;

        public AcessoDB()
        {
            var config = DependencyService.Get<IConfig>();
            conexaoSQLite = new SQLiteConnection(config.plataforma, Path.Combine(config.diretorioSQLite, "Devices.db3"));
            conexaoSQLite.CreateTable<DeviceModel>();
        }

        public void InserirDevice(DeviceModel device)
        {
            conexaoSQLite.Insert(device);
        }

        public void AtualizarDevice(DeviceModel device)
        {
            conexaoSQLite.Update(device);
        }

        public void DeletarDevice(DeviceModel device)
        {
            conexaoSQLite.Delete(device);
        }

        public DeviceModel GetDevice(string _devEUI)
        {
            return conexaoSQLite.Table<DeviceModel>().FirstOrDefault(c => c.deveui == _devEUI);
        }

        public ObservableCollection<DeviceModel> GetDevices()
        {
            var lista = conexaoSQLite.Table<DeviceModel>().OrderBy(c => c.deveui).ToList();
            return new ObservableCollection<DeviceModel>(lista);
        }

        public void Dispose()
        {
            conexaoSQLite.Dispose();
        }
    }
}
