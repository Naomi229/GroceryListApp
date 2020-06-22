using AsyncAwaitBestPractices.MVVM;
using GroceryList.Model;
using GroceryList.Service;
using GroceryList.Service.Persistance.Impl;
using GroceryList.ViewModels;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GroceryList.ViewModel
{
    public class ShowProductsListViewModel : ViewModelBase
    {

        private readonly INavigationService navigationService;
        private readonly GroceryService groceryService;
        private readonly ProximityService proximityService;
        private readonly IMessage message;

        public ShowProductsListViewModel(INavigationService navigationService, GroceryService groceryService, ProximityService proximityService, IMessage message) : base(navigationService)
        {
            Title = "Your shopping lists";
            this.navigationService = navigationService;
            this.groceryService = groceryService;
            this.proximityService = proximityService;
            this.message = message;

            Lists = new ObservableCollection<ProductsList>();


            LoadListsCommand = new AsyncCommand(LoadLists);


            if (CrossGeolocator.Current.IsListening)
            {
                CrossGeolocator.Current.StopListeningAsync();
                Debug.WriteLine("Stop listening async for location.");
            }
            //ListenerSettings listenerSettings = new ListenerSettings { AllowBackgroundUpdates = true };

            // Task.Run(() => StartListentinLocationAsync());

            //CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
            //CrossGeolocator.Current.PositionError += Current_PositionError;
        }

        private async Task StartListentinLocationAsync()
        {
            await GetLocationAsync(CrossGeolocator.Current);
        }

        async Task GetLocationAsync(IGeolocator locator)
        {
            await locator.StartListeningAsync(TimeSpan.FromSeconds(10), 10, false, new ListenerSettings { AllowBackgroundUpdates = true });
        }

   
        private void Current_PositionError(object sender, PositionErrorEventArgs e)
        {
            Debug.WriteLine("Curren location can't be readed");
        }

        private async void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            Debug.WriteLine($"LocationChangeEvent occured: lat={e.Position.Latitude}; long={e.Position.Longitude}");
            await proximityService.CheckProximityAsync(e.Position.Latitude, e.Position.Longitude);
        }

        private async Task LoadLists()
        {
            IsBusy = true;
            try
            {
                Lists.Clear();
                var items = await groceryService.GetAllProductsListAsync();
                
                foreach (var item in items)
                {                    
                    Lists.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ObservableCollection<ProductsList> Lists { get; set; }

     
        public ProductsList SelectedList{ get; set; }

        public ICommand LoadListsCommand { get; private set; }


        public ICommand CreateNewListCommand => new Command(async() => 
        {

            await navigationService.NavigateAsync("AddNewProductListPage", useModalNavigation: true);

        });

        public AsyncCommand<ProductsList> DeleteListCommand => new AsyncCommand<ProductsList>(async(prodList) =>
        {
            Debug.WriteLine($"List with id={prodList.Id} will be deleted");
            IsBusy = true;
            await groceryService.DeleteList(prodList);
            Lists.Remove(prodList);
            message.ShortAlert($"List {prodList.Name} deleted");
            IsBusy = false;
        });

        public ICommand SelectProductListCommand => new Command(async =>
        {
            var parms = new NavigationParameters();
            parms.Add("list", SelectedList);
            navigationService.NavigateAsync("ShowProductsPage", parms);
        });

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            LoadListsCommand.Execute(null);
        }
    }
}