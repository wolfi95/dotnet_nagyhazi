using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamanimation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fb.MC.Views
{
	public partial class MainPage : ContentPage
	{
        

        public MainPage()
        {
            MessagingCenter.Subscribe<LoginPageModel>(this, "Login",
               async (sender) =>
               {
                   await MPage.Animate(new ScaleToAnimation { Duration = "2000", Scale = 200 });
               });
            InitializeComponent();
        }
    }
}