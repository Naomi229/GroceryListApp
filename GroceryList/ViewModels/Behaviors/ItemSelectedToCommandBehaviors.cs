using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MVVMExample.ViewModel.Behaviors
{
    public class ItemSelectedToCommandBehaviors : Behavior<ListView>
    {

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(
                "Command",
                typeof(ICommand),
                typeof(ItemSelectedToCommandBehaviors));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value);  }
        }

        protected override void OnAttachedTo(ListView bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.ItemSelected += BindableOnItemSelected;
            bindable.BindingContextChanged += Bindable_BindingContextChanged;
        }

        private void Bindable_BindingContextChanged(object sender, EventArgs e)
        {
            var lv = sender as ListView;
            BindingContext = lv.BindingContext;
        }

        private void BindableOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Command.Execute(null);
        }

        protected override void OnDetachingFrom(ListView bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.ItemSelected -= BindableOnItemSelected;
            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
        }
    }
}
