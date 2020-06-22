using Prism;
using Prism.Ioc;
using GroceryList.ViewModels;
using GroceryList.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GroceryList.ViewModel;
using GroceryList.Service.Persistance.Impl;
using GroceryList.Service.Persistance;
using GroceryList.Model;
using GroceryList.Service;
using System.Diagnostics;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace GroceryList
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

           
            await NavigationService.NavigateAsync("NavigationPage/ShowProductsListPage");
        }


        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<AddNewProductListPage, AddNewProductsListViewModel>();
            containerRegistry.RegisterForNavigation<ShowProductsListPage, ShowProductsListViewModel>();
            containerRegistry.RegisterForNavigation<ShowProductsPage, ShowProductsViewModel>();
            containerRegistry.RegisterForNavigation<AddNewProductPage, AddNewProductViewModel>();
            

            containerRegistry.Register<IProdcutDataStore, SqlLiteProductDataStore>();
            containerRegistry.Register<IProductListDataStore, SqlLiteProductListDataStore>();
            containerRegistry.Register<GroceryService>();
            containerRegistry.Register<ProximityService>();



        }
    }
}
