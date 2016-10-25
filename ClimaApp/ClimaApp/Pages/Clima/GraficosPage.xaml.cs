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
using ClimaApp.Nivel;

namespace ClimaApp.Pages.Clima
{
    public partial class GraficosClimaPage : ContentPage
    {
        PlotView[] pv = new PlotView[4];
        LineSeries tempSeries = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 2, MarkerStroke = OxyColors.DarkRed, };
        LineSeries umidSeries = new LineSeries() { MarkerType = MarkerType.Circle, MarkerSize = 2, MarkerStroke = OxyColors.DarkBlue, };
        StairStepSeries presSeries = new StairStepSeries();
        RectangleBarSeries corrSeries = new RectangleBarSeries();

        NivelDevice node = new NivelDevice("", 1);

        DatePicker dp;
        Button leftButton, rightButton;
        const int graphSize = 300;
        int selectedIndex = DataResources.selectedIndex;

        public GraficosClimaPage()
        {
            ConfigureLayout();

            ToolbarItems.Add(new ToolbarItem("Dados", "", async () =>
            {
                await Navigation.PushAsync(new DadosClimaPage());
            }));

            var rng = new Random();

            for (int j = 0; j < 50; j++)
            {
                for (int i = 0; i < 24 * 4; i++)
                {
                    float random = (float)(rng.NextDouble() - 0.5f) * 5;
                    node.dados.Add(new NivelRx() { horario = DateTime.Today.AddMinutes(-15 * (i + 96* j)), nivel = (i >= 7 * 4 && i <= 19 * 4) ? 60 + random : 190 + random});
                }
            }



            if (dp.Date == dp.MinimumDate)
                leftButton.IsEnabled = false;

            if (dp.Date == dp.MaximumDate)
                rightButton.IsEnabled = false;

            DataResources.climaNodes[selectedIndex].SaveOnDb();
            //DataResources.climaNodes[selectedIndex].dados = new Common.Database.ClimaDb().GetDadosDevice(DataResources.climaNodes[selectedIndex].lora.deveui);
            AtualizarGraficos(dp.Date);
        }

        void ConfigureLayout()
        {
            Title = DataResources.climaNodes[selectedIndex].lora.comment;

            StackLayout screenLayout = new StackLayout() { Orientation = StackOrientation.Vertical, };

            StackLayout topLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, };
            {
                leftButton = new Button() { Text = "Anterior" };
                leftButton.Clicked += LeftButton_Clicked;
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

                rightButton = new Button() { Text = "Próximo" };
                rightButton.Clicked += RightButton_Clicked;
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

            //CORRENTE
            {
                pv[3] = new PlotView();
                PlotModel pm = new PlotModel() { Title = "Corrente", DefaultColors = new List<OxyColor>() { OxyColors.LightGray } };
                pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", IsPanEnabled = false, IsZoomEnabled = false, });
                pm.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Unit = "mA",
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineThickness = 2,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                corrSeries = new RectangleBarSeries();
                pm.Series.Add(corrSeries);
                pv[3].Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(graphSize, GridUnitType.Absolute) });
                graphLayout.Children.Add(pv[3], 0, 3);
            }

            ScrollView scrollView = new ScrollView() { Content = graphLayout, };

            screenLayout.Children.Add(topLayout);
            screenLayout.Children.Add(scrollView);
            Content = screenLayout;
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
            corrSeries.Items.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                tempSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].temperatura));
                umidSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].umidade));
                presSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(temp[i].horario), temp[i].pressao));

                var start = DateTimeAxis.ToDouble(temp[i].horario);
                var end = DateTimeAxis.ToDouble(temp[i].horario.AddMinutes(-15));

                corrSeries.Items.Add(new RectangleBarItem(start, 0, end, node.dados[i].nivel));
            }

            foreach (var pView in pv)
            {
                for (int i = 0; i < pView.Model.Axes.Count; i++)
                    pView.Model.Axes[i].Reset();
                pView.Model.InvalidatePlot(true);
            }
        }

        private void LeftButton_Clicked(object sender, EventArgs e)
        {
            rightButton.IsEnabled = true;

            dp.Date = dp.Date.AddDays(-1);
            AtualizarGraficos(dp.Date);

            if (dp.Date == dp.MinimumDate)
                leftButton.IsEnabled = false;
        }

        private void RightButton_Clicked(object sender, EventArgs e)
        {
            leftButton.IsEnabled = true;

            dp.Date = dp.Date.AddDays(1);
            AtualizarGraficos(dp.Date);

            if (dp.Date == dp.MaximumDate)
                rightButton.IsEnabled = false;
        }

        private void Dp_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarGraficos(dp.Date);
        }
    }
}
