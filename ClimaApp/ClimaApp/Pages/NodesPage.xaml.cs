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
            nodesView.ItemsSource = DataResources.allNodes;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            var selected = e.SelectedItem as DeviceModel;
            if (e.SelectedItem == null || selected.tipo != AppType.Clima)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            DataResources.climaSelecionado = new ClimaDevModel();
            DataResources.climaSelecionado.node = selected;
            await DataResources.climaSelecionado.PegarDados();
            await Navigation.PushModalAsync(new GraficosPage());
        }
    }
}
