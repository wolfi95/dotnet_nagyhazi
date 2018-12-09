using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Fb.MC.Behaviorss
{
    class ListViewNoSelectionBehavior : Behavior<ListView>
    {
        protected override void OnAttachedTo(ListView bindable)
        {
            bindable.ItemSelected += OnItemSelected;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            bindable.ItemSelected -= OnItemSelected;
            base.OnDetachingFrom(bindable);
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}
