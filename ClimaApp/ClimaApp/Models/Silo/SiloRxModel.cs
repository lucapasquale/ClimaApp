using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class SiloRxModel : RxModel
    {
        public int nCabos { get; private set; }
        public int[][] tempCabo { get; private set; } = new int[20][];

        public float tempExt { get; private set; }
        public float umidExt { get; private set; }

        //TODO: INCLUIR TX DA PARTE EXTERNA
        public override void ParseDataFrame()
        {
            /* dataFrame = a bbb cc dd + (bbb) * (EFGHIJKL) 

            a = 1 bit com o tipo de transmissão (Interna / Externa)
            bbb = 3 bits com numero de cabos desta transmissão
            cc = 2 bits com o total de transmissões
            dd = 2 bits como o numero da transmissão atual

            [SE FOR INTERNO]
            (bbb) * = Dado seguinte será repitido 'bbb' vezes, até um máximo de 5
            EFGHIJKL = 8 bytes sendo cada byte uma temperatura interna de um cabo */

            string bitsControle = Convert.ToString(Convert.ToInt32(dataFrame.Substring(0, 2), 16), 2).PadLeft(8, '0');
            bool interno = bitsControle.Substring(0, 1) == "1" ? true : false;
            if (interno)
            {
                int cabosTx = Convert.ToInt32(bitsControle.Substring(1, 3), 2);
                int totalTx = Convert.ToInt32(bitsControle.Substring(4, 2), 2);
                int txAtual = Convert.ToInt32(bitsControle.Substring(6, 2), 2);

                //Se for a ultima transmissão, pegar o total de cabos
                nCabos = txAtual == totalTx ? cabosTx + 5 * txAtual : 0;

                for (int i = 0; i < 5; i++)
                {
                    //Se ja transmitiu todos os cabos da TX parar
                    if (cabosTx >= i)
                        return;

                    int caboAtual = 5 * txAtual + i;
                    int posCabo = 2 + i * 16;

                    tempCabo[caboAtual] = new int[8];
                    for (int j = 0; j < 8; j++)
                    {
                        tempCabo[caboAtual][j] = int.Parse(dataFrame.Substring(posCabo + j * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
        }

    }
}
