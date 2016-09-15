using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class DadosClimaPage : ContentPage
    {
        ObservableCollection<ClimaRxModel> dados = new ObservableCollection<ClimaRxModel>();
        int selectedIndex = DataResources.selectedIndex;

        public DadosClimaPage()
        {
            InitializeComponent();
            Title = "Dados " + DataResources.climaNodes[selectedIndex].lora.comment;
            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.ClimaCell));
            nodesView.ItemsSource = dados;

            dp.MinimumDate = DataResources.climaNodes[selectedIndex].dados.Min(o => o.horario);
            dp.MaximumDate = DataResources.climaNodes[selectedIndex].dados.Max(o => o.horario);
            dp.Date = dp.MaximumDate;

            leftButton.Clicked += async (sender, e) =>
            {
                await LeftButton_Clicked(sender, e);
            };
            rightButton.Clicked += async (sender, e) =>
            {
                await RightButton_Clicked(sender, e);
            };

            AtualizaLista();
        }

        void AtualizaLista()
        {
            dados.Clear();

            for (int i = 0; i < DataResources.climaNodes[selectedIndex].dados.Count; i++)
                if (DataResources.climaNodes[selectedIndex].dados[i].horario.Date == dp.Date)
                    dados.Add(DataResources.climaNodes[selectedIndex].dados[i]);
        }

        private async Task LeftButton_Clicked(object sender, EventArgs e)
        {
            if (dp.Date == dp.MinimumDate)
            {
                await DisplayAlert("ERRO", "Não existem dados mais antigos!", "OK");
                (sender as Button).IsEnabled = false;
            }
            else
            {
                dp.Date = dp.Date.AddDays(-1);
                AtualizaLista();
                (sender as Button).IsEnabled = true;
            }
        }

        private async Task RightButton_Clicked(object sender, EventArgs e)
        {
            if (dp.Date == dp.MaximumDate)
            {
                await DisplayAlert("ERRO", "Não existem dados mais novos!", "OK");
                (sender as Button).IsEnabled = false;
            }
            else
            {
                dp.Date = dp.Date.AddDays(1);
                AtualizaLista();
                (sender as Button).IsEnabled = true;
            }
        }

        private async void nodesView_Refreshing(object sender, EventArgs e)
        {
            await DataResources.climaNodes[selectedIndex].GetData();

            leftButton.IsEnabled = true;
            rightButton.IsEnabled = true;

            dp.MaximumDate = DataResources.climaNodes[selectedIndex].dados.Max(o => o.horario);

            nodesView.IsRefreshing = false;
        }
    }
}
