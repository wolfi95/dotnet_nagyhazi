using Flatbuilder.DTO;
using FreshMvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamanimation;
using Xamarin.Forms;

namespace Fb.MC.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            MessagingCenter.Subscribe<LoginPageModel>(this, "InvalidChar",
                (sender) =>
                {
                    UserName.Animate(new ShakeAnimation { Duration = "1000"});
                }
                );
            MessagingCenter.Subscribe<LoginPageModel>(this, "Login",
                async (sender) =>
                {
                    await LPage.Animate(new FlipAnimation {Duration = "1000" });
                    sender.tcs.SetResult(true);
                });
            InitializeComponent();
        }
    }
}
