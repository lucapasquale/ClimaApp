using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class SiloRxModel : RxModel
    {
        public int idSilo { get; private set; }
        public int nSensores { get; private set; }
        public bool redeON { get; private set; }
        public bool secadorON { get; private set; }

        public float tempExt { get; private set; }
        public float umidExt { get; private set; }
        public List<float> tempInt { get; private set; }
        public List<float> umidInt { get; private set; }


        public override void ParseDataFrame()
        {
            /* dataFrame = aaa bbb c d + EEEE FFFF + (bbb) * GGGG HHHH

            aaa = 3 bits com o numero do silo
            bbb = 3 bits com o numero do sensor
            c = 1 bit se a rede eletrica caiu
            d = 1 bit se o secacador foi ligado
            EEEE = 4 bytes representando 10 * temperatura externa
            FFFF = 4 bytes representando 10 * umidade externa
            GGGG = 4 bytes representando 10 * temperatura interna (repetido bbb vezes)
            HHHH = 4 bytes representando 10 * umidade interna (repetido bbb vezes) */

            string bitsControle = Convert.ToString(Convert.ToInt32(dataFrame.Substring(0, 2), 16), 2).PadLeft(8, '0');

            idSilo = Convert.ToInt32(bitsControle.Substring(0, 3), 2);
            nSensores = Convert.ToInt32(bitsControle.Substring(3, 3), 2);
            redeON = bitsControle.Substring(6, 1) == "1" ? true : false;
            secadorON = bitsControle.Substring(7, 1) == "1" ? true : false;

            tempExt = int.Parse(dataFrame.Substring(2, 4), System.Globalization.NumberStyles.HexNumber) / 10f;
            umidExt = int.Parse(dataFrame.Substring(6, 4), System.Globalization.NumberStyles.HexNumber) / 10f;

            tempInt = new List<float>();
            umidInt = new List<float>();
            for (int i = 0; i < nSensores; i++)
            {
                if (10 + 8 * i > dataFrame.Length - 1)
                    return;

                tempInt.Add(int.Parse(dataFrame.Substring(10 + 8 * i, 4), System.Globalization.NumberStyles.HexNumber) / 10f);
                umidInt.Add(int.Parse(dataFrame.Substring(14 + 8 * i, 4), System.Globalization.NumberStyles.HexNumber) / 10f);
            }
        }
    }
}
