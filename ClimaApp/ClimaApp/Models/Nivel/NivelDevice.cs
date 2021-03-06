﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp.Nivel
{
    public class NivelDevice : DeviceModel<NivelRx>
    {
        public NivelDevice(string _name, float _nivel)
        {
            lora = new LoRaModel() { comment = _name, status = NodeStatus.Offline };
            latest = new NivelRx() { nivel = _nivel };
        }
    }
}
