using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Models
{
    public class User
    {
        [PrimaryKey]
        public string username { get; set; }
        public string email { get; set; }
        public DateTime lastVisit { get; set; }

        public string nodesDevEUI { get; set; }
    }
}
