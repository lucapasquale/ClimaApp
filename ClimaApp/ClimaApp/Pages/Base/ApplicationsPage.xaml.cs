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
            NavigationPage.SetHasBackButton(this, false);
            InitializeComponent();
            labelTitulo.Text = string.Format("Nodes usados: {0} / {1}", 
                (DataResources.allNodes.Count - DataResources.unnusedNodes), DataResources.allNodes.Count);

            ToolbarItems.Add(new ToolbarItem("Logoff", "", async () =>
            {
                StringResources.auth = null;
                StringResources.user = "";
                DataResources.ClearData();
                await Navigation.PopAsync();
            }));
        }

        private async void clima_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage("Atualizando módulos de clima"));
            {
                foreach (ClimaDevModel cDev in DataResources.climaNodes)
                    await cDev.GetLatest();
                await Navigation.PushAsync(new Clima.NodesClima());
            }
            await Navigation.PopModalAsync();
        }

        private void silos_clicked(object sender, EventArgs e)
        {

        }

        private async void atualizar_clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new LoadingPage("Atualizando todos os módulos"));
            {
                try
                {
                    await DataResources.GetNodes();
                }
                catch (System.Net.Http.HttpRequestException error)
                {
                    await DisplayAlert("Erro", error.Message, "OK");
                }
            }
            await Navigation.PopModalAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
