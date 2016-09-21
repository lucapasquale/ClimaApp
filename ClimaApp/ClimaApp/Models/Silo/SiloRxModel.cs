using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Models.Silo
{
    public class SiloRX : RxModel
    {
        public int nCabos { get; private set; }

        public float tempExt { get; private set; }
        public float umidExt { get; private set; }
        public float umidInt { get; private set; }
        
        public int[][] tempCabo { get; private set; }

        public float alturaEstimada { get; private set; }

        public override void ParseDataFrame()
        {
            /* dataFrame = aaaa xxxx + BBBB + CCCC + DDDD + (aaaa) * (EFGHIJKL) 

            aaaa = 4 bits representando o número de cabos de temp do silo
            xxxx = sem uso

            BBBB = 4 bytes com temperatura externa
            CCCC = 4 bytes com umidade externa
            DDDD = 4 bytes com umidade interna

            (aaaa) * = Dado seguinte será repitido 'aaaa' vezes
            EFGHIJKL = 8 bytes sendo cada byte uma temperatura interna de um cabo */

            string bitsControle = Convert.ToString(Convert.ToInt32(dataFrame.Substring(0, 2), 16), 2).PadLeft(8, '0');
            nCabos = Convert.ToInt32(bitsControle.Substring(0, 4), 2);
           
            tempExt = int.Parse(dataFrame.Substring(4, 4), NumberStyles.HexNumber) / 10f;
            umidExt = int.Parse(dataFrame.Substring(8, 4), NumberStyles.HexNumber) / 10f;
            umidInt = int.Parse(dataFrame.Substring(12, 4), NumberStyles.HexNumber) / 10f;

            tempCabo = new int[nCabos][];
            for (int i = 0; i < nCabos; i++)
            {
                tempCabo[i] = new int[8];

                int posCabo = 14 + i * 16;

                for (int j = 0; j < 8; j++)
                {
                    tempCabo[i][j] = int.Parse(dataFrame.Substring(posCabo + j * 2, 2), NumberStyles.HexNumber);
                }
            }

            GetAltura();
        }

        void GetAltura()
        {
            float[] alturas = new float[nCabos];
            for (int i = 0; i < nCabos; i++)
            {
                float maxDif = 0;
                for (int j = 0; j < 8 - 1; j++)
                {
                    float dif = tempCabo[i][j] - tempCabo[i][j + 1];
                    if (dif > maxDif)
                    {
                        alturas[i] = j + 0.5f;
                        maxDif = dif;
                    }
                }
            }

            alturaEstimada = alturas.Average();
        }
    }
}
