using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class ClimaRxModel : RxModel
    {
        public float temperatura { get; set; }
        public float umidade { get; set; }
        public int pressao { get; set; }
    }
}
