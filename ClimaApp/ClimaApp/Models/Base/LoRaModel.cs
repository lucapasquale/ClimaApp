using RestSharp.Portable;
using RestSharp.Portable.Authenticators;
using RestSharp.Portable.HttpClient;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public enum AppType { None = 0, Clima = 10, Silo = 30, Testes = 256, };
    public enum NodeStatus { Offline, Atrasado, Online }

    public class LoRaModel
    {
        [PrimaryKey]
        public string deveui { get; set; }
        public string last_reception { get; set; }
        public string appeui { get; set; }
        public string comment { get; set; }

        public DateTime horaUltimoRx { get; private set; }
        public AppType tipo { get; private set; } = AppType.None;
        public NodeStatus status { get; set; } = NodeStatus.Offline;
        public TimeSpan txInterval { get; set; }


        public void GetTipo()
        {
            horaUltimoRx = DateTime.Parse(last_reception);
            TimeZoneInfo.ConvertTime(horaUltimoRx, TimeZoneInfo.Local);

            if (comment.Contains("Balizador"))
            {
                tipo = AppType.Clima;
                txInterval = new TimeSpan(0, 0, 15, 0);
            }

            if (comment.Contains("Silo"))
            {
                tipo = AppType.Silo;
                txInterval = new TimeSpan(0, 0, 30, 0);
            }

            GetStatus();  
        }

        public void GetStatus()
        {
            double minutesFromLatest = DateTime.Now.Subtract(horaUltimoRx).TotalMinutes;
            status = NodeStatus.Offline;

            if (minutesFromLatest < txInterval.TotalMinutes * 10)
                status = NodeStatus.Atrasado;

            if (minutesFromLatest < txInterval.TotalMinutes * 2.5)
                status = NodeStatus.Online;
        }
    }
}
