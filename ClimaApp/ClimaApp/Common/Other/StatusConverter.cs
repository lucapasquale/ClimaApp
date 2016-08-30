using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (NodeStatus)value;
            switch (status)
            {
                case NodeStatus.ONLINE:
                    return Color.Green;
                case NodeStatus.ATRASADO:
                    return Color.Yellow;
                case NodeStatus.OFFLINE:
                    return Color.Red;
            }

            return Color.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
