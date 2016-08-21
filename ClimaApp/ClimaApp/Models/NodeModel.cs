using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public class NodeModel
    {
        public string devEUI { get; set; }
        public int device_status { get; set; }
        public int dl_fcnt { get; set; }
        public int device_class { get; set; }
        public int registration_status { get; set; }
        public int expiry_time_uplink { get; set; }
        public int expiry_time_downlink { get; set; }
        public int max_allowed_dutycycle { get; set; }
        public int expected_avr_dutycycle { get; set; }
        public int quo_class { get; set; }
        public string last_reception { get; set; }
        public string appEUI { get; set; }
        public string coment { get; set; }
        public float latitude { get; set; }
        public float longitude { get; set; }
    }
}
