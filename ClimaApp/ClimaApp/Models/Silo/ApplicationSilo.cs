using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Silo
{
    public class ApplicationSilo
    {
        public string appName;

        //public VentDevice ventilador;
        public Dictionary<string, SiloDevice> silos;

        public ApplicationSilo()
        {
            appName = "Fazenda Sapé";
            foreach (SiloDevice siloDev in DataResources.siloNodes)
                silos.Add(siloDev.lora.deveui, siloDev);

            //foreach (VentDevice ventDev in DataResources.ventNodes)
            //    silos.Add(ventDev.lora.deveui, ventDev);
        }
    }
}
