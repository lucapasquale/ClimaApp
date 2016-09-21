using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages.Base
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            StringResources.auth = new RestSharp.Portable.Authenticators.HttpBasicAuthenticator(usernameEntry.Text, passwordEntry.Text);
            StringResources.user = usernameEntry.Text;

            await Navigation.PushModalAsync(new LoadingPage("Logando em Orbiwise"));
            {
                try
                {
                    await DataResources.GetNodes();
                    await Navigation.PushAsync(new ApplicationsPage());
                }

                catch (System.Net.Http.HttpRequestException error)
                {
                    await DisplayAlert("Erro", "Conta ou senha errados!", "OK");
                    passwordEntry.Focus();
                }
            }
            await Navigation.PopModalAsync();
        }
    }
}
