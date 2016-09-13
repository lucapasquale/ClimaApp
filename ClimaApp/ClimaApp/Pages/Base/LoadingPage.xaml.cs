using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class LoadingPage : ContentPage
    {
        public string substring { get; set; }
        public int count { get; set; }
        public int totalCount { get; set; }

        public LoadingPage(string _substring)
        {
            substring = _substring;

            InitializeComponent();
            BindingContext = this;
            label2.SetBinding(Label.TextProperty, "substring");
        }

        public LoadingPage(string _substring, int _totalCount)
        {
            substring = _substring;
            totalCount = _totalCount;

            InitializeComponent();
            BindingContext = this;
            label2.SetBinding(Label.TextProperty, "substring");
            label3.SetBinding(Label.TextProperty, "count");
            label4.SetBinding(Label.TextProperty, new Binding(path: "totalCount", stringFormat: " / {0}"));
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
