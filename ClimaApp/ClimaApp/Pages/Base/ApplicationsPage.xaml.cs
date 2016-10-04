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
        List<ApplicationModel> apps = new List<ApplicationModel>();

        public ApplicationsPage()
        {
            NavigationPage.SetHasBackButton(this, false);
            ToolbarItems.Add(new ToolbarItem("Logoff", "", async () =>
            {
                StringResources.auth = null;
                StringResources.user = "";
                DataResources.ClearData();
                await Navigation.PopAsync();
            }));
            InitializeComponent();
            labelTitulo.Text = string.Format("Nodes usados: {0} / {1}", (DataResources.allNodes.Count - DataResources.unnusedNodes), DataResources.allNodes.Count);


            apps.Add(new ApplicationModel(AppType.Clima));
            apps.Add(new ApplicationModel(AppType.Silo));
            lv.ItemsSource = apps;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            switch((e.SelectedItem as ApplicationModel).type)
            {
                case AppType.Clima:
                    {
                        await Navigation.PushModalAsync(new LoadingPage("Atualizando módulos de clima"));
                        {
                            foreach (ClimaDevModel cDev in DataResources.climaNodes)
                                await cDev.GetLatest();
                            await Navigation.PushAsync(new Clima.NodesClima());
                        }
                        await Navigation.PopModalAsync();
                        break;
                    }
                case AppType.Silo:
                    {
                        await Navigation.PushModalAsync(new LoadingPage("Atualizando módulos de silo"));
                        {
                            await Navigation.PushAsync(new Silo.SiloAppPage());
                        }
                        await Navigation.PopModalAsync();
                        break;
                    }  
            }
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
