﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using OxyPlot;
using OxyPlot.Xamarin.Forms;
using OxyPlot.Axes;
using OxyPlot.Series;
using System.Diagnostics;
using UIKit;

namespace ClimaApp.Pages
{
    public partial class GraficosClimaPage : ContentPage
    {
        PlotView[] pv = new PlotView[3];
        LineSeries tempSeries = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 2, MarkerStroke = OxyColors.DarkRed, };
        LineSeries umidSeries = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 2, MarkerStroke = OxyColors.DarkBlue, };
        StairStepSeries presSeries = new StairStepSeries();

        DatePicker dp;
        const int graphSize = 300;
        int selectedIndex = DataResources.selectedIndex;

        public GraficosClimaPage()
        {
            ConfigureLayout();

            ToolbarItems.Add(new ToolbarItem("Dados", "", async () =>
            {
                await Navigation.PushAsync(new DadosClimaPage());
            }));

            AtualizarGraficos(dp.Date);
        }

        void ConfigureLayout()
        {
            Title = DataResources.climaNodes[selectedIndex].lora.comment;

            StackLayout screenLayout = new StackLayout() { Orientation = StackOrientation.Vertical, };

            StackLayout topLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, };
            {
                //topLayout.Children.Add(new Label() { Text = "Data:", FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand, });

                Button leftButton = new Button() { Text = "Voltar" };
                leftButton.Clicked += async (sender, e) =>
                {
                    await LeftButton_Clicked(sender, e);
                };
                topLayout.Children.Add(leftButton);

                dp = new DatePicker()
                {
                    Format = "dd-MM-yyyy",
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    MinimumDate = DataResources.climaNodes[selectedIndex].dados.Min(o => o.horario),
                    MaximumDate = DataResources.climaNodes[selectedIndex].dados.Max(o => o.horario),
                };
                dp.Date = dp.MaximumDate;
                dp.DateSelected += Dp_DateSelected;
                topLayout.Children.Add(dp);

                Button rightButton = new Button() { Text = "Avançar" };
                rightButton.Clicked += async (sender, e) =>
                {
                    await RightButton_Clicked(sender, e);
                };
                topLayout.Children.Add(rightButton);
            }

            Grid graphLayout = new Grid();
            //TEMPERATURA
            {
                pv[0] = new PlotView();
                PlotModel pm = new PlotModel() { Title = "Temperatura", DefaultColors = new List<OxyColor>() { OxyColors.Red } };
                pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", IsPanEnabled = false, IsZoomEnabled = false, });
                pm.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Unit = "ºC",
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineThickness = 2,
                    MinorGridlineStyle = LineStyle.Automatic,
                    MinorGridlineThickness = 1,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                pm.Series.Add(tempSeries);
                pv[0].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(graphSize, GridUnitType.Absolute) });
                graphLayout.Children.Add(pv[0], 0, 0);
            }

            //UMIDADE
            {
                pv[1] = new PlotView();
                PlotModel pm = new PlotModel() { Title = "Umidade", DefaultColors = new List<OxyColor>() { OxyColors.Blue } };
                pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", IsPanEnabled = false, IsZoomEnabled = false, });
                pm.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Unit = "%",
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineThickness = 2,
                    MinorGridlineStyle = LineStyle.Automatic,
                    MinorGridlineThickness = 1,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                pm.Series.Add(umidSeries);
                pv[1].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(graphSize, GridUnitType.Absolute) });
                graphLayout.Children.Add(pv[1], 0, 1);
            }

            //PRESSAO
            {
                pv[2] = new PlotView();
                PlotModel pm = new PlotModel() { Title = "Pressão", DefaultColors = new List<OxyColor>() { OxyColors.Green } };
                pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", IsPanEnabled = false, IsZoomEnabled = false, });
                pm.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Unit = "hPa",
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineThickness = 2,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                presSeries = new StairStepSeries();
                pm.Series.Add(presSeries);
                pv[2].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(graphSize, GridUnitType.Absolute) });
                graphLayout.Children.Add(pv[2], 0, 2);
            }
            ScrollView scrollView = new ScrollView() { Content = graphLayout, };

            screenLayout.Children.Add(topLayout);
            screenLayout.Children.Add(scrollView);
            Content = screenLayout;
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
                AtualizarGraficos(dp.Date);
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
                AtualizarGraficos(dp.Date);
                (sender as Button).IsEnabled = true;
            }
        }

        void AtualizarGraficos(DateTime dia)
        {
            var temp = new List<ClimaRxModel>();

            for (int i = 0; i < DataResources.climaNodes[selectedIndex].dados.Count; i++)
                if (DataResources.climaNodes[selectedIndex].dados[i].horario.Date == dia)
                    temp.Add(DataResources.climaNodes[selectedIndex].dados[i]);

            tempSeries.Points.Clear();
            umidSeries.Points.Clear();
            presSeries.Points.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                tempSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].temperatura));
                umidSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].umidade));
                presSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].pressao));
            }

            foreach (var pView in pv)
            {
                pView.Model.Axes[0].Reset();
                pView.Model.Axes[1].Reset();
                pView.Model.InvalidatePlot(true);
            }
        }

        private void Dp_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarGraficos(dp.Date);
        }
    }
}
