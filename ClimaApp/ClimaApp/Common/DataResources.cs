using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClimaApp
{
    public static class DataResources
    {
        public static ClimaDevModel nodeTCC = new ClimaDevModel();

        public static ObservableCollection<DeviceModel> nodes = new ObservableCollection<DeviceModel>();

        public static ObservableCollection<SiloDevModel> siloNodes = new ObservableCollection<SiloDevModel>();
    }
}
