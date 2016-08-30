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
    public class DevicesDb : IDisposable
    {
        private SQLiteConnection conSQLDevice;

        public DevicesDb()
        {
            var config = DependencyService.Get<IConfig>();
            conSQLDevice = new SQLiteConnection(config.plataforma, Path.Combine(config.diretorioSQLite, "Devices.db3"));
            conSQLDevice.CreateTable<LoRaModel>();
        }

        public void InserirDevice(LoRaModel device)
        {
            conSQLDevice.Insert(device);
        }

        public void AtualizarDevice(LoRaModel device)
        {
            conSQLDevice.Update(device);
        }

        public void DeletarDevice(LoRaModel device)
        {
            conSQLDevice.Delete(device);
        }

        public LoRaModel GetDevice(string _devEUI)
        {
            return conSQLDevice.Table<LoRaModel>().FirstOrDefault(c => c.deveui == _devEUI);
        }

        public List<LoRaModel> GetDevices()
        {
            return conSQLDevice.Table<LoRaModel>().OrderBy(c => c.comment).ToList();
        }

        public void Dispose()
        {
            conSQLDevice.Dispose();
        }
    }
}
