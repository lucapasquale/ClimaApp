using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Nivel
{
    public class NivelRx : RxModel
    {
        public float nivel { get; set; }

        public override void ParseDataFrame()
        {
            base.ParseDataFrame();

            nivel = int.Parse(dataFrame.Substring(0, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
        }
    }
}
