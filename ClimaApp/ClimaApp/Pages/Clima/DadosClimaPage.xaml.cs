using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Clima
{
    public partial class DadosClimaPage : ContentPage
    {
        ObservableCollection<ClimaRxModel> dados = new ObservableCollection<ClimaRxModel>();
        int selectedIndex = DataResources.selectedIndex;

        public DadosClimaPage()
        {
            InitializeComponent();
            Title = "Dados " + DataResources.climaNodes[selectedIndex].lora.comment;
            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.Clima.ClimaCell));
            nodesView.ItemsSource = dados;

            dp.MinimumDate = DataResources.climaNodes[selectedIndex].dados.Min(o => o.horario);
            dp.MaximumDate = DataResources.climaNodes[selectedIndex].dados.Max(o => o.horario);
            dp.Date = dp.MaximumDate;

            leftButton.Clicked += LeftButton_Clicked;
            rightButton.Clicked += RightButton_Clicked;

            if (dp.Date == dp.MinimumDate)
                leftButton.IsEnabled = false;

            if (dp.Date == dp.MaximumDate)
                rightButton.IsEnabled = false;

            AtualizaLista();
        }

        void AtualizaLista()
        {
            dados.Clear();

            for (int i = 0; i < DataResources.climaNodes[selectedIndex].dados.Count; i++)
                if (DataResources.climaNodes[selectedIndex].dados[i].horario.Date == dp.Date)
                    dados.Add(DataResources.climaNodes[selectedIndex].dados[i]);
        }

        private void LeftButton_Clicked(object sender, EventArgs e)
        {
            rightButton.IsEnabled = true;

            dp.Date = dp.Date.AddDays(-1);
            AtualizaLista();

            if (dp.Date == dp.MinimumDate)
                leftButton.IsEnabled = false;
        }

        private void RightButton_Clicked(object sender, EventArgs e)
        {
            leftButton.IsEnabled = true;

            dp.Date = dp.Date.AddDays(1);
            AtualizaLista();

            if (dp.Date == dp.MaximumDate)
                rightButton.IsEnabled = false;
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
