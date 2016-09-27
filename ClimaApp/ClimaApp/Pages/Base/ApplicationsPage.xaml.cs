using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

        private async void enviar_clicked(object sender, EventArgs e)
        {
            Debug.WriteLine("sending data");
            await DataResources.climaNodes[0].SendData(new byte[] { 0x01, 0x02, 0x03, 0x0A, 0x0B, 0x0C });
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
