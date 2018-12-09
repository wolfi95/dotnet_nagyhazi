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
            MessagingCenter.Subscribe<MainPageModel>(this, "Click",
               async (sender) =>
               {
                   await MPage.Animate(new ScaleToAnimation { Duration = "4000", Scale = 200 });
                   sender.tcs.SetResult(true);
               });
            InitializeComponent();
        }
    }
}