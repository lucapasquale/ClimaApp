﻿using System;
using System.Collections.Generic;
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
            NavigationPage.SetHasNavigationBar(this, false);
            InitializeComponent();
            labelTitulo.Text = "Total de nodes: " + new DevicesDb().GetModulos().Count.ToString();
        }

        private async void clima_clicked(object sender, EventArgs e)
        {
            foreach (ClimaDevModel cDev in DataResources.climaNodes)
                await cDev.GetLatest();

            await Navigation.PushAsync(new Clima.NodesClima());
        }

        private async void silos_clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Clima.NodesClima());
        }

        private async void atualizar_clicked(object sender, EventArgs e)
        {
            await DataResources.GetNodes();
        }
    }
}
