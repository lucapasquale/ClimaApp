using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ClimaApp.Cells.Silo
{
    public class SiloDeviceCell : ViewCell
    {
        public SiloDeviceCell()
        {
            StackLayout cellSL = new StackLayout() { Spacing = 5, Padding = 5 };

            StackLayout titleSL = new StackLayout() { Orientation = StackOrientation.Horizontal };
            {
                Label titleLabel = new Label() { FontSize = 20, HorizontalOptions = LayoutOptions.StartAndExpand, FontAttributes = FontAttributes.Bold };
                titleLabel.SetBinding(Label.TextProperty, "lora.comment");
                titleSL.Children.Add(titleLabel);

                Label statusLabel = new Label() { VerticalOptions = LayoutOptions.EndAndExpand };
                statusLabel.SetBinding(Label.TextProperty, "lora.status");
                statusLabel.SetBinding(Label.TextColorProperty, new Binding(path: "lora.status", converter: new StatusConverter()));
                titleSL.Children.Add(statusLabel);
            }
            cellSL.Children.Add(titleSL);

            StackLayout bottomSL = new StackLayout() { Orientation = StackOrientation.Horizontal, VerticalOptions = LayoutOptions.EndAndExpand };
            {
                Label alturaLabel = new Label();
                alturaLabel.SetBinding(Label.TextProperty, new Binding(path: "latest.alturaEstimada", stringFormat: "Altura estimada: {0} metros"));
                bottomSL.Children.Add(alturaLabel);

                bottomSL.Children.Add(new Label() { Text = "Ventilador:", HorizontalOptions = LayoutOptions.EndAndExpand });

                Label ventiladorLabel = new Label() { HorizontalOptions = LayoutOptions.End, FontAttributes = FontAttributes.Bold };
                ventiladorLabel.SetBinding(Label.TextProperty, new Binding(path: "latest.ventiladorOn", converter: new Common.Other.BoolColorValueConverter()));
                bottomSL.Children.Add(ventiladorLabel);
            }
            cellSL.Children.Add(bottomSL);

            View = cellSL;
        }
    }
}
