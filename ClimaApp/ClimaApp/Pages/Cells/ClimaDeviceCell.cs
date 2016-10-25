using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp.Cells.Clima
{
    public class ClimaDeviceCell : ViewCell
    {
        public ClimaDeviceCell()
        {
            StackLayout screenSL = new StackLayout();

            StackLayout titleSL = new StackLayout() { Orientation = StackOrientation.Horizontal };
            {
                Label titleLabel = new Label() { FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand, FontAttributes = FontAttributes.Bold };
                titleLabel.SetBinding(Label.TextProperty, "lora.comment");
                titleSL.Children.Add(titleLabel);

                Label statusLabel = new Label() { FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center};
                statusLabel.SetBinding(Label.TextProperty, "lora.status");
                statusLabel.SetBinding(Label.TextColorProperty, new Binding(path: "lora.status", converter: new StatusConverter()));
                titleSL.Children.Add(statusLabel);
            }
            screenSL.Children.Add(titleSL);

            StackLayout bottomSL = new StackLayout() { Orientation = StackOrientation.Horizontal, };
            {
                StackLayout dataSL = new StackLayout();
                {
                    Label tempLabel = new Label();
                    tempLabel.SetBinding(Label.TextProperty, new Binding(path: "latest.temperatura", stringFormat: "Temp: {0}ºC"));
                    dataSL.Children.Add(tempLabel);

                    Label umidLabel = new Label();
                    umidLabel.SetBinding(Label.TextProperty, new Binding(path: "latest.umidade", stringFormat: "Umid: {0}%"));
                    dataSL.Children.Add(umidLabel);

                    Label presLabel = new Label();
                    presLabel.SetBinding(Label.TextProperty, new Binding(path: "latest.pressao", stringFormat: "Pres: {0}hPa"));
                    dataSL.Children.Add(presLabel);
                }
                bottomSL.Children.Add(dataSL);

                StackLayout horarioSL = new StackLayout() { HorizontalOptions = LayoutOptions.EndAndExpand };
                {
                    Label horaLabel = new Label() { VerticalOptions = LayoutOptions.EndAndExpand };
                    horaLabel.SetBinding(Label.TextProperty, new Binding(path: "latest.horario", stringFormat: "{0: HH:mm - dd/MM/yy}"));
                    horarioSL.Children.Add(horaLabel);
                }
                bottomSL.Children.Add(horarioSL);
            }
            screenSL.Children.Add(bottomSL);

            View = screenSL;
        }
    }
}
