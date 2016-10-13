using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Nivel
{
    public partial class NivelApp : ContentPage
    {
        public NivelApp()
        {
            InitializeComponent();
            nodesView.ItemsSource = DataResources.nivelNodes;
        }

        void ListView_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            
        }
    }
}
