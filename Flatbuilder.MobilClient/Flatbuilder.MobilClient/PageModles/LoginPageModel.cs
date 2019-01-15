using AspNetCore.Http.Extensions;
using Fb.MC.Views;
using Flatbuilder.DTO;
using FreshMvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Fb.MC.Views
{
    public class LoginPageModel : FreshBasePageModel
    {
        public TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();

        static readonly Uri baseAddress = new Uri("http://10.0.2.2:51502/");

        public ICommand LoginCommand { private set; get; }
        private string userName;
        public string UserName
        {
            get
            {
                return userName;
            }
            set
            {
                userName = value;
                RaisePropertyChanged("UserName");
                if (userName != "")
                    if (!ValidateUsername())
                        MessagingCenter.Send(this, "InvalidChar");
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }
        public LoginPageModel()
        {
            LoginCommand = new Command(
               execute: async () =>
                {
                    if (!ValidateUsername())
                    {
                        await Application.Current.MainPage.DisplayAlert("Invalid Username", "Username can't contain any special characters or whitespaces!", "Ok");
                        return;
                    }
                    Costumer costumer;
                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = baseAddress;

                        MessagingCenter.Send(this, "Login");

                        var response = await client.GetAsync("api/Costumer/get/" + UserName);
                        if(response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            costumer = new Costumer() { Name = UserName };
                            var res = await client.PostAsJsonAsync<Costumer>("api/Costumer/create", costumer);
                            if(res.StatusCode != System.Net.HttpStatusCode.Created)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong!", "Ok");
                                return;
                            }
                            string json = await response.Content.ReadAsStringAsync();
                            costumer = JsonConvert.DeserializeObject<Costumer>(json);
                        }
                    }
                    var navpage = new FreshNavigationContainer(FreshPageModelResolver.ResolvePageModel<MainPageModel>(costumer));
                    await tcs.Task;
                    Application.Current.MainPage = navpage;
                },
               canExecute:() => 
               {
                   if (UserName == "" || UserName == null)
                   {
                       return false;
                   }
                   else return true;
               });
        }
       
        public override void Init(object initData)
        {
            base.Init(initData);
           
        }

        private bool ValidateUsername()
        {
            if (!Regex.IsMatch(UserName, "^[a-zA-Z0-9]*$"))
                return false;

            return true;
        }
    }
}
