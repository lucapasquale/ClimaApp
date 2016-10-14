using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Nivel
{
    public partial class NivelApp : ContentPage
    {
        public NivelApp()
        {
            InitializeComponent();
            nodesView.ItemsSource = DataResources.nivelNodes;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null


            await Navigation.PushModalAsync(new LoadingPage("Carregando gráfico"));
            {
                DataResources.selectedIndex = DataResources.nivelNodes.FindIndex(a => a == e.SelectedItem as ClimaApp.Nivel.NivelDevice);
                await Navigation.PushAsync(new NivelGraph());
            }
            await Navigation.PopModalAsync();
        }
    }
}
