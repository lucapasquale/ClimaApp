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
        public static ClimaDevModel climaSelecionado;

        public static ObservableCollection<DeviceModel> allNodes = new ObservableCollection<DeviceModel>();
        public static ObservableCollection<ClimaDevModel> climaNodes = new ObservableCollection<ClimaDevModel>();
        public static ObservableCollection<SiloDevModel> siloNodes = new ObservableCollection<SiloDevModel>();
        public static ObservableCollection<DeviceModel> testeNodes = new ObservableCollection<DeviceModel>();
    }
}
