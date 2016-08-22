using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class NodesPage : ContentPage
    {

        public NodesPage()
        {
            InitializeComponent();
            nodesView.ItemsSource = DataResources.nodes;
        }
    }
}
