using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class RxModel
    {
        public long Id { get; set; }
        public string timeStamp { get; set; }
        public string dataFrame { get; set; }
        public int fcnt { get; set; }
        public int port { get; set; }
        public int rssi { get; set; }
        public float snr { get; set; }
        public int sr_used { get; set; }
        public bool decrypted { get; set; }

        public DateTime horario { get; set; }

        public virtual void ParseDataFrame() { }
    }
}
