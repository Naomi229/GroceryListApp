using GroceryList.Model;
using GroceryList.Service;
using GroceryList.Service.Persistance;
using GroceryList.ViewModels;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GroceryList.ViewModel
{
    public class AddNewProductsListViewModel : ViewModelBase
    {
        private readonly INavigationService navigationService;
        private readonly GroceryService groceryService;

        public AddNewProductsListViewModel(INavigationService navigationService, GroceryService groceryService) : base(navigationService)
        {
            Title = "Create new List";
            this.navigationService = navigationService;
            this.groceryService = groceryService;
            SaveListCommand = new Command(async () => await SaveList());
        }

        public ICommand SaveListCommand { get; }



        private string listName;
        public string Name
        {
            get { return listName; }
            set { SetProperty(ref listName, value); }
        }
        private async Task SaveList()
        {

            groceryService.AddNewProductList(Name);
            Debug.WriteLine("Add new productList: Name=" + Name);
            await navigationService.GoBackAsync();
        }
    }
}
