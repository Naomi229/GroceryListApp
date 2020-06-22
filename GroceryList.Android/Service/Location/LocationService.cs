using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase;
using Firebase.Iid;
using Firebase.Messaging;
using GroceryList.Service;
using Newtonsoft.Json.Linq;
using Org.Json;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Prism.Services;

namespace GroceryList.Droid.Service.Location
{
    [Service(Exported = false, ForegroundServiceType = Android.Content.PM.ForegroundService.TypeLocation, Name = "GroceryList.Droid.Service.Location.LocationService")]
    public class LocationService : Android.App.Service
    {
        private App _app => (App)Xamarin.Forms.Application.Current;

        const string TAG = "LocationService";
        private ProximityService proximityService;

        public LocationService()
        {
           proximityService = (ProximityService)_app.Container.Resolve(typeof(ProximityService));
        }


    public override IBinder OnBind(Intent intent)
        {
            return null;
            //throw new NotImplementedException();
        }


        public override void OnCreate()
        {

            Log.Debug(TAG, "Create service LocationService.");
            ListenerSettings listenerSettings = new ListenerSettings { AllowBackgroundUpdates = true };


            CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(10), 10, false, new ListenerSettings { AllowBackgroundUpdates = true });

            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;

            Log.Debug(TAG, "Service created. Listen for location Changed");

        }


        private async void Current_PositionChanged(object sender, PositionEventArgs e)
        {
            Log.Debug(TAG, $"new Location({e.Position.Latitude}, {e.Position.Longitude}");
            // new GroceryList.Droid.Services.NotificationHelper().CreateNotification("newNotific", $"new Location({e.Position.Latitude}, {e.Position.Longitude}");

            await proximityService.CheckProximityAsync(e.Position.Latitude, e.Position.Longitude);

        }
    }
}