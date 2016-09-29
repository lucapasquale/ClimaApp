using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp.Cells.Clima
{
    class ClimaCell : ViewCell
    {
        public ClimaCell()
        {
            StackLayout cellLayout = new StackLayout() { BackgroundColor = Color.FromHex("#eee"), Orientation = StackOrientation.Horizontal };
            StackLayout valoresLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.Start };

            StackLayout[] layoutSensores = new StackLayout[3];
            Label[] labelSensores = new Label[9];
            string[] textos = { " Temperatura:\t", "temperatura", "ºC", " Umidade:\t", "umidade", "%", " Pressão:\t", "pressao", "hPa" };

            //TEMPERATURA, UMIDADE E PRESSAO
            for (int i = 0; i < labelSensores.Length; i++)
            {
                if (i % 3 == 0)
                    layoutSensores[i / 3] = new StackLayout() { Orientation = StackOrientation.Horizontal };

                labelSensores[i] = new Label() { Text = textos[i] };
                if (i % 3 == 1)
                    labelSensores[i].SetBinding(Label.TextProperty, textos[i]);

                layoutSensores[i / 3].Children.Add(labelSensores[i]);
                valoresLayout.Children.Add(layoutSensores[i / 3]);
            }

            //HORARIO
            StackLayout horarioLayout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            Label intensidadeLabel = new Label() { TextColor = Color.Maroon, HorizontalOptions = LayoutOptions.EndAndExpand };
            intensidadeLabel.SetBinding(Label.TextProperty, "rssi");

            Label horarioLabel = new Label() { HorizontalOptions = LayoutOptions.EndAndExpand, VerticalOptions = LayoutOptions.End };
            horarioLabel.SetBinding(Label.TextProperty, new Binding(path: "horario", stringFormat: "{0: HH:mm - dd/MM/yy}"));

            horarioLayout.Children.Add(intensidadeLabel);
            horarioLayout.Children.Add(horarioLabel);


            cellLayout.Children.Add(valoresLayout);
            cellLayout.Children.Add(horarioLayout);
            View = cellLayout;
        }
    }
}
