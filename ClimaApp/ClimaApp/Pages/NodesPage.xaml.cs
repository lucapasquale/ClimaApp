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

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            if (selected.tipo == AppType.None)
            {
                await DisplayAlert("ERRO", "Nenhum dado no servidor para este módulo!", "ok");
                return;
            }

            DataResources.climaSelecionado.node = selected;
            await DataResources.climaSelecionado.PegarDados();
            await Navigation.PushModalAsync(new GraficosPage());
        }
    }
}
