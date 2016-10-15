using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Silo
{
    public class SiloRX : RxModel
    {
        public bool ventiladorOn { get; set; }
        public int alturaEstimada { get; set; }

        public float tempGrao { get; set; }
        public float tempExt { get; set; }

        public float umidInt { get; set; }
        public float umidExt { get; set; }

        public override void ParseDataFrame()
        {
            /* dataFrame = abbb bbbb + BBBB + CCCC + DDDD + (aaaa) * (EFGHIJKL) 

            a = 1 bit representando se o ventilador esta ligado ou desligado
            bbb bbbb = 7 bits representando o uso do silo em %

            CCCC = 4 bytes com temperatura dos graos
            DDDD = 4 bytes com temperatura externa

            EEEE = 4 bytes com umidade interna
            FFFF = 4 bytes com umidade externa */

            string bitsControle = Convert.ToString(Convert.ToInt32(dataFrame.Substring(0, 2), 16), 2).PadLeft(8, '0');

            ventiladorOn = bitsControle.Substring(0, 1) == "1" ? true : false;
            alturaEstimada = Convert.ToInt32(bitsControle.Substring(1,7), 2);

            tempGrao = int.Parse(dataFrame.Substring(2, 4), NumberStyles.HexNumber) / 10f;
            tempExt = int.Parse(dataFrame.Substring(6, 4), NumberStyles.HexNumber) / 10f;

            umidInt = int.Parse(dataFrame.Substring(10, 4), NumberStyles.HexNumber) / 10f;
            umidExt = int.Parse(dataFrame.Substring(14, 4), NumberStyles.HexNumber) / 10f;
        }

        //void GetAltura()
        //{
        //    float[] alturas = new float[nCabos];
        //    for (int i = 0; i < nCabos; i++)
        //    {
        //        float maxDif = 0;
        //        for (int j = 0; j < 8 - 1; j++)
        //        {
        //            float dif = tempCabo[i][j] - tempCabo[i][j + 1];
        //            if (dif > maxDif)
        //            {
        //                alturas[i] = j + 0.5f;
        //                maxDif = dif;
        //            }
        //        }
        //    }

        //    alturaEstimada = alturas.Average();
        //}
    }
}
