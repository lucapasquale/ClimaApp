using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class NodesPage : ContentPage
    {
        public NodesPage()
        {
            InitializeComponent();
            nodesView.ItemsSource = DataResources.nodes;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            var selected = e.SelectedItem as DeviceModel;
            if (e.SelectedItem == null || selected.tipo != AppType.Clima)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            await DataResources.nodeTCC.PegarDados(StringResources.devEUIarduino);
            await Navigation.PushModalAsync(new GraficosPage());
        }
    }
}
