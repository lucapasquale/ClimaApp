using ClimaApp.Nivel;
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

namespace ClimaApp.Pages.Nivel
{
    public partial class NivelGraph : ContentPage
    {
        public NivelDevice node { get; set; }

        PlotView pv = new PlotView();
        RectangleBarSeries rbs = new RectangleBarSeries();

        Button leftButton, rightButton;
        DatePicker dp;
        int graphSize = 300;
        DateTime graphDay = DateTime.Now;

        public NivelGraph()
        {
            node = DataResources.nivelNodes[DataResources.selectedIndex];
            InitializeComponent();
            ConfigureLayout();

            node.dados.Clear();
            var rng = new Random();
            for (int j = 0; j < 24 * 14; j++)
                node.dados.Add(new NivelRx() { horario = graphDay.AddHours(-j), nivel = node.latest.nivel + node.latest.nivel * 0.1f * ((float)rng.NextDouble() - 0.5f) });

            PlotGraph();
        }

        void ConfigureLayout()
        {
            Title = node.lora.comment;

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
                    Date = DateTime.Now,
                    MinimumDate = DateTime.Now.AddDays(-28),
                    MaximumDate = DateTime.Now,
                };
                dp.Date = dp.MaximumDate;
                dp.DateSelected += Dp_DateSelected;
                topLayout.Children.Add(dp);

                rightButton = new Button() { Text = "Próximo" };
                rightButton.Clicked += RightButton_Clicked;
                topLayout.Children.Add(rightButton);
            }
            screenLayout.Children.Add(topLayout);

            Grid graphLayout = new Grid();
            {
                PlotModel pm = new PlotModel() { Title = "Nível d'água", DefaultColors = new List<OxyColor>() { OxyColors.LightBlue } };
                pm.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "HH:mm", IsPanEnabled = false, IsZoomEnabled = false, });
                pm.Axes.Add(new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Unit = "m",
                    MajorGridlineStyle = LineStyle.Solid,
                    MajorGridlineThickness = 2,
                    IsPanEnabled = false,
                    IsZoomEnabled = false,
                });
                pm.Series.Add(rbs);
                pv.Model = pm;
                graphLayout.RowDefinitions.Add(new RowDefinition { Height = new GridLength(graphSize, GridUnitType.Absolute) });
                graphLayout.Children.Add(pv, 0, 0);
            }
            screenLayout.Children.Add(graphLayout);

            Content = screenLayout;
        }

        void PlotGraph()
        {
            rbs.Items.Clear();

            //DEBUG
            var temp = new List<NivelRx>();
            for (int i = 0; i < node.dados.Count; i++)
                if (node.dados[i].horario.Day == dp.Date.Day)
                    temp.Add(node.dados[i]);


            for (int i = 0; i < temp.Count; i++)
            {
                var start = DateTimeAxis.ToDouble(temp[i].horario);
                var end = DateTimeAxis.ToDouble(temp[i].horario.AddHours(-1));

                rbs.Items.Add(new RectangleBarItem(start, 0, end, temp[i].nivel));
            }

            //var temp = new List<NivelRx>();
            //for (int j = 0; j < node.dados.Count; j++)
            //    if (node.dados[j].horario.Day == dp.Date.Day)
            //        temp.Add(node.dados[j]);

            //for (int i = 0; i < temp.Count; i++)
            //{
            //    var start = DateTimeAxis.ToDouble(temp[i].horario);
            //    var end = DateTimeAxis.ToDouble(temp[i].horario.AddHours(-1));

            //    rbs.Items.Add(new RectangleBarItem(start, 0, end, temp[i].nivel));
            //}
            for (int i = 0; i < pv.Model.Axes.Count; i++)
                pv.Model.Axes[i].Reset();
            pv.Model.InvalidatePlot(true);
        }

        private void LeftButton_Clicked(object sender, EventArgs e)
        {
            rightButton.IsEnabled = true;

            dp.Date = dp.Date.AddDays(-1);
            PlotGraph();

            if (dp.Date == dp.MinimumDate)
                leftButton.IsEnabled = false;
        }

        private void RightButton_Clicked(object sender, EventArgs e)
        {
            leftButton.IsEnabled = true;

            dp.Date = dp.Date.AddDays(1);
            PlotGraph();

            if (dp.Date == dp.MaximumDate)
                rightButton.IsEnabled = false;
        }

        private void Dp_DateSelected(object sender, DateChangedEventArgs e)
        {
            PlotGraph();
        }
    }
}
