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
        public Dictionary<string, SiloDevice> silos = new Dictionary<string, SiloDevice>();

        public ApplicationSilo()
        {
            appName = "Fazenda Sapé";
            //foreach (SiloDevice siloDev in DataResources.siloNodes)
            //    silos.Add(siloDev.lora.deveui, siloDev);

            //foreach (VentDevice ventDev in DataResources.ventNodes)
            //    silos.Add(ventDev.lora.deveui, ventDev);

            silos.Add("1", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "012345678", comment = "Silo 1", status = NodeStatus.Atrasado },
                latest = new SiloRX() { alturaEstimada = 4.5f, ventiladorOn = false }
            });

            silos.Add("2", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "ABCDEFGH", comment = "Silo 2", status = NodeStatus.Online },
                latest = new SiloRX() { alturaEstimada = 3.2f, ventiladorOn = true }
            });

            silos.Add("3", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "ABCDEFGH", comment = "Silo 3", status = NodeStatus.Offline },
                latest = new SiloRX() { alturaEstimada = 3.7f, ventiladorOn = false }
            });

            silos.Add("4", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "ABCDEFGH", comment = "Silo 4", status = NodeStatus.Online },
                latest = new SiloRX() { alturaEstimada = 1.9f, ventiladorOn = true }
            });
        }
    }
}
