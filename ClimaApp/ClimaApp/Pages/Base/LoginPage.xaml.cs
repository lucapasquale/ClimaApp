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
            await Navigation.PushModalAsync(new LoadingPage("Logando em Orbiwise"));
            {
                string uName = usernameEntry.Text.ToLower();

                try
                {
                    StringResources.restClient.Authenticator = new RestSharp.Portable.Authenticators.HttpBasicAuthenticator(uName, passwordEntry.Text);
                    StringResources.username = uName;
                    await DataResources.GetNodes();

                    await Task.Run(() =>
                    {
                        var db = new Common.Database.UserDb();
                        var user = new Models.User() { username = uName, lastVisit = DateTime.Now };

                        //Converte todos os devEUI ligados a conta em uma string separados por virgula
                        user.nodesDevEUI = string.Join(",", DataResources.allNodes.Select(c => c.deveui).ToList());
                        Debug.WriteLine(user.nodesDevEUI);

                        db.conexaoSQLite.InsertOrReplace(user);
                        StringResources.loggedUser = user;
                        StringResources.isLoggedIn = true;
                    });

                    await Navigation.PushAsync(new ApplicationsPage());
                }

                catch (Exception error)
                {
                    await DisplayAlert(error.Message, error.Message, "OK");
                    passwordEntry.Text = "";
                    passwordEntry.Focus();
                }
            }
            await Navigation.PopModalAsync();
        }
    }
}
