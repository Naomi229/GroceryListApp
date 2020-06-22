using GroceryList.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace GroceryList.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewProductPage : ContentPage
    {

        public AddNewProductPage()
        {
            InitializeComponent();
        }

        private async Task<Position> GetLastKnownPos()
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location == null)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default, TimeSpan.FromSeconds(30));
                location = await Geolocation.GetLocationAsync(request);
            }

            return new Position(location.Latitude, location.Longitude);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Map.MoveToRegion(MapSpan.FromCenterAndRadius(await GetLastKnownPos(), Distance.FromKilometers(2)));
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {

            addPin(e.Position.Latitude, e.Position.Longitude, "Product location");

            Debug.WriteLine($"Pin location: lat={e.Position.Latitude}; long={e.Position.Longitude}");

            if (this.BindingContext is AddNewProductViewModel vmm)
            {
                vmm.ProductLocation = new Location(e.Position.Latitude, e.Position.Longitude);
            }
            
        }

        private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var locations = await Geocoding.GetLocationsAsync(e.NewTextValue.ToString());
                var location = locations?.FirstOrDefault();
                if (location != null)
                {
                    Debug.WriteLine($"New Location from Geocoding: lat={location.Latitude}; lon={location.Longitude}");
                    addPin(location.Latitude, location.Longitude, "location for the address");



                    if (this.BindingContext is AddNewProductViewModel vm)
                    {

                        vm.ProductLocation = new Location(location.Latitude, location.Longitude);
                        Map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMeters(300)));
                    }

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception occured when typing address. Message: {ex.Message}");
            }
        }

        private void addPin(double lat, double lon, string label)
        {
            Map.Pins.Clear();
            Pin p = new Pin() { Position = new Position(lat, lon), Label = label};
            Map.Pins.Add(p);
        }
    }
}