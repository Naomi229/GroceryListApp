using AsyncAwaitBestPractices.MVVM;
using GroceryList.Model;
using GroceryList.Service;
using GroceryList.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GroceryList.ViewModel
{
    public class AddNewProductViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly GroceryService groceryService;
        private readonly IMessage message;

        // public Map Map { get; private set; }

        public AddNewProductViewModel(INavigationService navigationService, GroceryService groceryService, IMessage message) : base(navigationService)
        {
            Title = "Add new Product";
            this.navigationService = navigationService;
            this.groceryService = groceryService;
            this.message = message;

        }

        public IMessage Message { get; }

        private string productName;
        public string ProductName
        {
            get { return productName; }
            set { SetProperty(ref productName, value); }
        }

        private int quntity;
        public int Quantity
        {
            get { return quntity; }
            set { SetProperty(ref quntity, value); }
        }

        private Location productLocation;
        public Location ProductLocation 
        { 
            get { return productLocation; }
            set { SetProperty(ref productLocation, value); }
        }

        private ProductsList parentProductList { get; set; }


        public ICommand SaveProduct => new AsyncCommand(async() =>
        {
            IsBusy = true;
            if (ProductLocation == null)
            {
                message.ShortAlert("Please select a location!!");
                return;
            }

            await groceryService.AddNewProduct(ProductName, Quantity, productLocation, false, parentProductList.Id);
            Debug.WriteLine("Add new Product: ProductName = " + ProductName + "; Quanitty: "+ Quantity);
            message.LongAlert($"Product {ProductName} was added!");
            var parms = new NavigationParameters();
            parms.Add("list", parentProductList);
            IsBusy = false;
            await navigationService.GoBackAsync(parms);
            
            
        });

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = true;
            base.OnNavigatedTo(parameters);
            parentProductList = parameters["productList"] as ProductsList;
            IsBusy = false;

        }
    }
}
