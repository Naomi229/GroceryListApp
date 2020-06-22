using GroceryList.Model;
using GroceryList.Service;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace GroceryList.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly GroceryService GroceryService;
        public MainPageViewModel(INavigationService navigationService, GroceryService groceryService)
            : base(navigationService)
        {
            Title = "Main Page";
            this.GroceryService = groceryService;
            //Debug.WriteLine(GroceryService.GetServiceNameAsync());
            DelegateCommand = new DelegateCommand(OnLaunch);
        }

        private void OnLaunch()
        {
            Debug.WriteLine("List: id="+productsList.Id +"; name:"+productsList.Name);
           
        }

        public DelegateCommand DelegateCommand { get; }


        private ProductsList productsList;
        public ProductsList ProductsList 
        {
            get { return productsList; }
            set { SetProperty(ref productsList, value); }
        }
      
    }
}
