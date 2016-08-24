using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            nodesView.ItemsSource = DataResources.climaSelecionado.dados;
        }

        private async void nodesView_Refreshing(object sender, EventArgs e)
        {
            await DataResources.climaSelecionado.PegarDados();
            Debug.WriteLine(DataResources.climaSelecionado.node.deveui);
            nodesView.IsRefreshing = false;
        }
    }
}
