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
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            var dados = new AcessoDB();
            nodesView.ItemsSource = dados.GetDevices();
        }

        private async void nodesView_Refreshing(object sender, EventArgs e)
        {
            var dados = new AcessoDB();
            dados.Dispose();
            await DeviceModel.PegarNodes();

            nodesView.IsRefreshing = false;
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
            await Navigation.PushAsync(new GraficosPage());
        }
    }
}
