using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Silo
{
    public class ApplicationSilo : ApplicationModel
    {
        public string appName;

        public Dictionary<string, SiloDevice> silos = new Dictionary<string, SiloDevice>();
        public static List<SiloVentConfig> configs = new List<SiloVentConfig>();

        public ApplicationSilo()
        {
            appName = "Fazenda Sapé";
            //foreach (SiloDevice siloDev in DataResources.siloNodes)
            //    silos.Add(siloDev.lora.deveui, siloDev);

            silos.Add("1", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "012345678", comment = "Silo 1", status = NodeStatus.Atrasado },
                latest = new SiloRX() { alturaEstimada = 45, ventiladorOn = false, tempGrao = 25.3f, umidExt = 47.3f, umidInt = 42.2f },
                siloConfig = new SiloConfig("Milho", 12.5f, 7),
                ventConfig = new SiloVentConfig("Configuração padrão", 5)
            });

            silos.Add("2", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "ABCDEFGH", comment = "Silo 2", status = NodeStatus.Online },
                latest = new SiloRX() { alturaEstimada = 32, ventiladorOn = true }
            });

            silos.Add("3", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "ABCDEFGH", comment = "Silo 3", status = NodeStatus.Offline },
                latest = new SiloRX() { alturaEstimada = 37, ventiladorOn = false }
            });

            silos.Add("4", new SiloDevice()
            {
                lora = new LoRaModel() { deveui = "ABCDEFGH", comment = "Silo 4", status = NodeStatus.Online },
                latest = new SiloRX() { alturaEstimada = 19, ventiladorOn = true }
            });

            configs.Add(new SiloVentConfig("Milho", 3));
            configs.Add(new SiloVentConfig("Soja", 12));
        }
    }
}
