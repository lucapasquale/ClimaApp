using ClimaApp.Silo;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Silo
{
    public partial class SiloAppPage : ContentPage
    {
        public ApplicationSilo appSilo { get; set; } = DataResources.appSilo;

        public SiloAppPage()
        {
            InitializeComponent();

            titleLabel.Text = appSilo.appName;
            countLabel.Text = string.Format("Total de silos: {0}", appSilo.silos.Count);

            listaSilo.ItemsSource = appSilo.silos.Values;
            listaSilo.ItemTemplate = new DataTemplate(typeof(Cells.Silo.SiloDeviceCell));
            listaSilo.ItemSelected += async (sender, args) => await ListaSilo_ItemSelected(sender, args);
        }

        private async Task ListaSilo_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null

            var selected = e.SelectedItem as SiloDevice;

            DataResources.siloSelecionado = selected;
            await Navigation.PushAsync(new SiloConfigPage());
        }
    }
}
