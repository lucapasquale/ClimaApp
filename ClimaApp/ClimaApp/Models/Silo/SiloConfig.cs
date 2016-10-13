using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Silo
{
    public class SiloConfig
    {
        public string configName;

        public int difUmidade;

        public SiloConfig() { }

        public SiloConfig(string _name, int _umid)
        {
            configName = _name;
            difUmidade = _umid;
        }
    }
}
