using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class DadosClimaPage : ContentPage
    {
        public DadosClimaPage()
        {
            InitializeComponent();
            Title = "Dados " + DataResources.climaSelecionado.node.comment;
            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.ClimaCell));
            nodesView.ItemsSource = DataResources.climaSelecionado.dados;
        }

        private async void nodesView_Refreshing(object sender, EventArgs e)
        {
            await DataResources.climaSelecionado.GetData();
            Debug.WriteLine(DataResources.climaSelecionado.node.deveui);
            nodesView.IsRefreshing = false;
        }
    }
}
