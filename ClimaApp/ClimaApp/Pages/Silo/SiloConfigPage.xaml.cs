using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using ClimaApp.Silo;

namespace ClimaApp.Pages.Silo
{
    public partial class SiloConfigPage : ContentPage
    {
        public SiloDevice silo { get; set; } = DataResources.siloSelecionado;

        public SiloConfigPage()
        {
            BindingContext = silo;
            InitializeComponent();
        }
    }
}