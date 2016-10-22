using SQLite.Net;
using Xamarin.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ClimaApp.Models;

namespace ClimaApp.Common.Database
{
    public class UserDb
    {
        public SQLiteConnection conexaoSQLite;

        public UserDb()
        {
            var config = DependencyService.Get<IConfig>();
            conexaoSQLite = new SQLiteConnection(config.plataforma, Path.Combine(config.diretorioSQLite, "users.db3"));
            conexaoSQLite.CreateTable<User>();
        }

        public User GetUser(string _username)
        {
            return conexaoSQLite.Table<User>().FirstOrDefault(c => c.username == _username);
        }
    }
}
