using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class DadosPage : ContentPage
    {
        public DadosPage()
        {
            InitializeComponent();
            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.DadosCell));
            nodesView.ItemsSource = DataResources.node.dados;
        }

        private async void nodesView_Refreshing(object sender, EventArgs e)
        {
            await DataResources.node.PegarDados(StringResources.devEUIarduino);
            nodesView.IsRefreshing = false;
        }
    }
}
