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
                        title = "Clima em Balizadores";
                        description = "Medição meteorológica automática";
                        iconLocation = "clima_icon.png";
                        type = AppType.Clima;
                        break;
                    }
                case AppType.Silo:
                    {
                        title = "Silos Agrícolas";
                        description = "Controle de ventilação de grãos em silos";
                        iconLocation = "silo_icon.png";
                        type = AppType.Silo;
                        break;
                    }

                case AppType.Nivel:
                    {
                        title = "Nível de água";
                        description = "Verificação do nível de caixas d'água";
                        iconLocation = "nivel_icon.png";
                        type = AppType.Nivel;
                        break;
                    }
            }
        }
    }
}
