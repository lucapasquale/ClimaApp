using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ClimaApp.Pages
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            StringResources.restClient.Authenticator = new RestSharp.Portable.Authenticators.HttpBasicAuthenticator(usernameEntry.Text, passwordEntry.Text);
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
                    await DisplayAlert(error.Message, "Conta ou senha errados!", "OK");
                    passwordEntry.Focus();
                }
            }
            await Navigation.PopModalAsync();
        }
    }
}
