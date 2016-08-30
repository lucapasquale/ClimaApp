using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Clima
{
    public partial class NodesClima : ContentPage
    {
        DevicesDb db = new DevicesDb();

        public NodesClima()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.ClimaDeviceCell));
            nodesView.ItemsSource = DataResources.climaNodes;
            nodesView.RowHeight = 105;
        }
    }
}
