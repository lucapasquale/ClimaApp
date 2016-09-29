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
        int selectedIndex = DataResources.selectedIndex;

        public NodesPage()
        {
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            nodesView.ItemsSource = DataResources.allNodes;
            nodesView.IsRefreshing = false;
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
                    DataResources.climaNodes[selectedIndex].lora = selected;
                    await DataResources.climaNodes[selectedIndex].GetData();
                    await Navigation.PushAsync(new Clima.GraficosClimaPage());
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
