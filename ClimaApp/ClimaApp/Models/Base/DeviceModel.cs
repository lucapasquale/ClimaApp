using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable.Authenticators;
using System.Collections.ObjectModel;
using SQLite.Net.Attributes;

namespace ClimaApp
{
    public enum AppType {None = 0, Clima = 10, Silo = 30, Testes = 256, };

    public class DeviceModel
    {
        [PrimaryKey]
        public string deveui { get; set; }
        public string last_reception { get; set; }
        public string appeui { get; set; }
        public string comment { get; set; }

        public AppType tipo { get; set; }
        public DateTime dataUltimoRx { get; set; }
    }
}
