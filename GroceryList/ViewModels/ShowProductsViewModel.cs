using GroceryList.Model;
using GroceryList.Service;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace GroceryList.ViewModels
{
    public class ShowProductsViewModel : ViewModelBase
    {

        private readonly INavigationService navigationService;
        private readonly GroceryService groceryService;
        private readonly IPageDialogService dialogService;
        private readonly IMessage message;
        public ShowProductsViewModel(INavigationService navigationService, GroceryService groceryService, IPageDialogService dialogService, IMessage message) : base(navigationService)
        {

            this.navigationService = navigationService;
            this.groceryService = groceryService;
            this.dialogService = dialogService;
            this.message = message;
            ProductList = new ObservableCollection<Product>();
            LoadProductsCommand = new DelegateCommand(LoadProducts);
            

            // LoadProductsCommand.Execute(null);

        }

        private async void LoadProducts()
        {
            try
            {
                ProductList.Clear();
                var items = await groceryService.GetAllProductFromListAsync(ParentProductsList.Id);
               
                foreach (var item in items)
                {
                    ProductList.Add(item);
                    Debug.WriteLine("Product: Name=" + item.Name + "; id=" + item.Id);
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

       
        public ProductsList ParentProductsList { get; private set; }

        private Product selectedProduct;
        public Product SelectedProduct
        { 
            get { return selectedProduct; }
            set { SetProperty(ref selectedProduct, value); } 
        }

        public ObservableCollection<Product> ProductList { get; set; }
       
        public ICommand LoadProductsCommand { get; set; }

     

        public ICommand CreateNewProductCommand => new Command(async () => 
        {
            NavigationParameters parms = new NavigationParameters();
            parms.Add("productList", ParentProductsList);
            await navigationService.NavigateAsync("AddNewProductPage", parms);
        });

        public ICommand SelectedProdutCommand => new Command(async() =>
        {
            
            SelectedProduct.Bought = !SelectedProduct.Bought;
            await groceryService.ChangeProductBoughtStatus(SelectedProduct, SelectedProduct.Bought);
            
            message.ShortAlert(SelectedProduct.Bought == true? $"{SelectedProduct.Name} was marked as bought": $"{SelectedProduct.Name} was marked as not bought");
           // await dialogService.DisplayAlertAsync("Alert", $"{SelectedProduct.Name} was marked as Bought?{SelectedProduct.Bought} ", "Ok");
            LoadProductsCommand.Execute(null);
        });

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.ContainsKey("list"))
            {
                ParentProductsList = parameters["list"] as ProductsList;

                Title = "Products from list " + ParentProductsList.Name;
            }
             
            LoadProductsCommand.Execute(null);

            Debug.WriteLine("List: id=" + ParentProductsList.Id + "; name:" + ParentProductsList.Name);
        }




    }
}
