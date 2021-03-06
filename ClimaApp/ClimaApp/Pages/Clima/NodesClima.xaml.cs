﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Clima
{
    public partial class NodesClima : ContentPage
    {
        DevicesDb db = new DevicesDb();

        public NodesClima()
        {
            InitializeComponent();

            nodesView.ItemTemplate = new DataTemplate(typeof(Cells.Clima.ClimaDeviceCell));
            nodesView.ItemsSource = DataResources.climaNodes;
        }

        async void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;

            if (e.SelectedItem == null)
                return; //ItemSelected is called on deselection, which results in SelectedItem being set to null


            await Navigation.PushModalAsync(new LoadingPage("Carregando gráficos"));
            {
                DataResources.selectedIndex = DataResources.climaNodes.FindIndex(a => a == e.SelectedItem as ClimaDevModel);
                var selectedIndex = DataResources.selectedIndex;

                var db = new Common.Database.ClimaDb();
                DataResources.climaNodes[selectedIndex].dados = db.GetDadosDevice(DataResources.climaNodes[selectedIndex].lora.deveui);

                //Server está vazio
                if (DataResources.climaNodes[selectedIndex].latest == null)
                {
                    //Database também vazio
                    if (db.GetDadosDevice(DataResources.climaNodes[selectedIndex].lora.deveui).Count == 0)
                    {
                        await DisplayAlert("ERRO", "Não existem dados no servidor nem no banco de dados!", "OK");
                        await Navigation.PopModalAsync();
                        return;
                    }
                    else
                        await Task.Run(() => { DataResources.climaNodes[selectedIndex].dados = db.GetDadosDevice(DataResources.climaNodes[selectedIndex].lora.deveui); });
                    
                }
                else
                {
                    //Se o latest não existe no db, pegar dados do servidor. Se existe pegar do db
                    if (db.GetDadoHora(DataResources.climaNodes[selectedIndex].latest.horario) == null)
                    {
                        Debug.WriteLine("É preciso atualizar dados, pegando do servidor");
                        await Task.Run(async () => { await DataResources.climaNodes[selectedIndex].GetData(); });
                    }
                    else
                    {
                        Debug.WriteLine("Dados já atualizados, pegando do database");
                        await Task.Run(() => { DataResources.climaNodes[selectedIndex].dados = db.GetDadosDevice(DataResources.climaNodes[selectedIndex].lora.deveui); });
                    }
                }
                await Navigation.PushAsync(new GraficosClimaPage());
            }
            await Navigation.PopModalAsync();
        }
    }
}
