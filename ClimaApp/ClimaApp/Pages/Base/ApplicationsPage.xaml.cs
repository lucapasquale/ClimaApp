using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class ApplicationsPage : ContentPage
    {
        public ApplicationsPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();
            labelTitulo.Text = "Total de nodes: " + new DevicesDb().GetModulos().Count.ToString();
        }

        private async void clima_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage());

            foreach (ClimaDevModel cDev in DataResources.climaNodes)
                await cDev.GetLatest();
            await Navigation.PushAsync(new Clima.NodesClima());

            await Navigation.PopModalAsync();
        }

        private void silos_clicked(object sender, EventArgs e)
        {
            
        }

        private async void atualizar_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage());

            await DataResources.GetNodes();

            await Navigation.PopModalAsync();
        }
    }
}
