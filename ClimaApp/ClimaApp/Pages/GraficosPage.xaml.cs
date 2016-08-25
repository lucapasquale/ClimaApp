using System;
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

namespace ClimaApp.Pages
{
    public partial class GraficosPage : ContentPage
    {
        PlotView[] pv = new PlotView[3];
        LineSeries tempSeries = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 2, MarkerStroke = OxyColors.DarkRed, };
        LineSeries umidSeries = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 2, MarkerStroke = OxyColors.DarkBlue, };
        StairStepSeries presSeries = new StairStepSeries();

        DatePicker dp;

        public GraficosPage()
        {
            ConfigureLayout();
            AtualizarGraficos(dp.Date);
        }

        void ConfigureLayout()
        {
            StackLayout screenLayout = new StackLayout() { Orientation = StackOrientation.Vertical, };

            StackLayout topLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, };
            {
                topLayout.Children.Add(new Label() { Text = "Data:", FontSize = 20, VerticalOptions = LayoutOptions.CenterAndExpand, });

                dp = new DatePicker()
                {
                    Format = "dd-MM-yyyy",
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    MinimumDate = DataResources.climaSelecionado.dados.Min(o => o.horario),
                    MaximumDate = DataResources.climaSelecionado.dados.Max(o => o.horario),
                };
                dp.Date = dp.MaximumDate;
                dp.DateSelected += Dp_DateSelected;
                topLayout.Children.Add(dp);

                var button = new Button() { Text = "Ver Dados", HorizontalOptions = LayoutOptions.EndAndExpand, };
                button.Clicked += Button_Clicked;
                topLayout.Children.Add(button);

                screenLayout.Children.Add(topLayout);
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
                    MajorGridlineThickness = 2,
                    MinorGridlineStyle = LineStyle.Dash,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                pm.Series.Add(tempSeries);
                pv[0].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(350, GridUnitType.Absolute) });
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
                    MajorGridlineThickness = 2,
                    MinorGridlineStyle = LineStyle.Dash,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                pm.Series.Add(umidSeries);
                pv[1].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(350, GridUnitType.Absolute) });
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
                    MajorGridlineThickness = 2,
                    MinorGridlineStyle = LineStyle.Dash,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                presSeries = new StairStepSeries();
                pm.Series.Add(presSeries);
                pv[2].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(350, GridUnitType.Absolute) });
                graphLayout.Children.Add(pv[2], 0, 2);
            }
            ScrollView scrollView = new ScrollView() { Content = graphLayout, };

            screenLayout.Children.Add(scrollView);
            Content = screenLayout;
        }

        void AtualizarGraficos(DateTime dia)
        {
            var temp = new List<ClimaRxModel>();

            for (int i = 0; i < DataResources.climaSelecionado.dados.Count; i++)
                if (DataResources.climaSelecionado.dados[i].horario.Date == dia)
                    temp.Add(DataResources.climaSelecionado.dados[i]);

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

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var modalPage = new DadosPage();
            await Navigation.PushModalAsync(modalPage);
            Debug.WriteLine("The modal page is now on screen");
        }

        private void Dp_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarGraficos(dp.Date);
        }
    }
}
