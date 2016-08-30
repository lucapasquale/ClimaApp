using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class NodesPage : ContentPage
    {
        DevicesDb db = new DevicesDb();

        public NodesPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            DataResources.allNodes = db.GetDevices();
            nodesView.ItemsSource = DataResources.allNodes;
        }

        private async void nodesView_Refreshing(object sender, EventArgs e)
        {
            db.Dispose();
            await DataResources.GetNodes();

            nodesView.IsRefreshing = false;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
            var selected = e.SelectedItem as LoRaModel;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            switch (selected.tipo)
            {
                case AppType.Clima:
                    DataResources.climaSelecionado.node = selected;
                    await DataResources.climaSelecionado.GetData();
                    await Navigation.PushAsync(new GraficosClimaPage());
                    break;

                case AppType.Testes:
                    await DisplayAlert("ERRO", "Não é possível ver a lista de dados teste!", "ok");
                    break;

                default:
                    await DisplayAlert("ERRO", "Nenhum dado no servidor para este módulo!", "ok");
                    return;
            }
        }
    }
}
