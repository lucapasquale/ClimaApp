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
            //NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();

            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.ClimaDeviceCell));
            nodesView.ItemsSource = DataResources.climaNodes;
            nodesView.RowHeight = 105;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            await Navigation.PushModalAsync(new LoadingPage());

            DataResources.climaSelecionado = e.SelectedItem as ClimaDevModel;
            await DataResources.climaSelecionado.GetData();
            await Navigation.PushAsync(new GraficosClimaPage());

            await Navigation.PopModalAsync();
        }
    }
}
