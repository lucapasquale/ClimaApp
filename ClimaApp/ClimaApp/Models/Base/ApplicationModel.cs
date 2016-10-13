using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp
{
    public class ApplicationModel
    {
        public string title { get; set; }
        public AppType type { get; set; }

        public string description { get; set; }
        public string iconLocation { get; set; }

        public ApplicationModel() {}

        public ApplicationModel(AppType _type)
        {
            switch (_type)
            {
                case AppType.Clima:
                    {
                        title = "Clima";
                        description = "Medição meteorológica automática";
                        iconLocation = "icon.png";
                        type = AppType.Clima;
                        break;
                    }
                case AppType.Silo:
                    {
                        title = "Silos";
                        description = "Controle de ventilação de silos";
                        iconLocation = "icon.png";
                        type = AppType.Silo;
                        break;
                    }

                case AppType.Nivel:
                    {
                        title = "Nível";
                        description = "Altura da caixa d'água";
                        iconLocation = "icon.png";
                        type = AppType.Nivel;
                        break;
                    }
            }
        }
    }
}
