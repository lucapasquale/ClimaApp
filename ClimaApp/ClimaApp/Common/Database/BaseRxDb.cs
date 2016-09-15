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
    public abstract class BaseRxDb<T> : IDisposable
        where T : RxModel, new()
    {
        protected SQLiteConnection conexaoSQLite;

        public void InserirDado(T _dado)
        {
            conexaoSQLite.Insert(_dado);
        }

        public void AtualizarDado(T _dado)
        {
            conexaoSQLite.Update(_dado);
        }

        public void DeletarDado(T _dado)
        {
            conexaoSQLite.Delete(_dado);
        }

        public T GetDado(long _Id)
        {
            return conexaoSQLite.Table<T>().FirstOrDefault(c => c.Id == _Id);
        }

        public T GetDadoHora(DateTime _horario)
        {
            return conexaoSQLite.Table<T>().FirstOrDefault(c => c.horario == _horario);
        }

        public List<T> GetDadosDevice(string _devEUI)
        {
            var temp = new List<T>();
            foreach (T rx in conexaoSQLite.Table<T>())
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
