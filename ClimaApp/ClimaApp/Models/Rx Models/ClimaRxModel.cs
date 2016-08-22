using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class ClimaRxModel : RxModel
    {
        public float temperatura { get; private set; }
        public float umidade { get; private set; }
        public int pressao { get; private set; }

        public override void ParseDataFrame()
        {
            //Converte de HEX para decimal
            temperatura = int.Parse(dataFrame.Substring(0, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
            umidade = int.Parse(dataFrame.Substring(4, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
            pressao = int.Parse(dataFrame.Substring(8, 4), System.Globalization.NumberStyles.HexNumber);
        }
    }
}
