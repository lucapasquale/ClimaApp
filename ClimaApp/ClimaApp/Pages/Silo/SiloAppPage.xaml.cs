using ClimaApp.Silo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Silo
{
    public partial class SiloAppPage : ContentPage
    {
        List<SiloDevice> silos = new List<SiloDevice>();

        public SiloAppPage()
        {
            InitializeComponent();
            CreateLayout();
        }

        void CreateLayout()
        {
            StackLayout screenSL = new StackLayout() { Padding = 5 };
            {
                Label appNameLabel = new Label() { Text = DataResources.appSilo.appName, FontSize = 30 };
                screenSL.Children.Add(appNameLabel);

                Label qntdSilo = new Label() { Text = "Total de silos: " + DataResources.appSilo.silos.Count };
                screenSL.Children.Add(qntdSilo);

                screenSL.Children.Add(new BoxView() { Color = Color.Gray, WidthRequest = 100, HeightRequest = 1.5 });

                ListView listaSilo = new ListView()
                {
                    ItemsSource = DataResources.appSilo.silos.Values,
                    ItemTemplate = new DataTemplate(typeof(Cells.Silo.SiloDeviceCell)),
                    RowHeight = 70
                };
                listaSilo.ItemSelected += async (sender, args) => await ListaSilo_ItemSelected(sender, args);
                screenSL.Children.Add(listaSilo);
            }
            Content = screenSL;
        }

        private async Task ListaSilo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            var selected = e.SelectedItem as SiloDevice;

            await DisplayAlert("ATENÇÂO", "Deseja ligar o ventilador de " + selected.lora.comment + "?", "Sim", "Cancelar");
        }
    }
}
