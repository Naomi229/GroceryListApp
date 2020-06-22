using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using GroceryList.Droid.Service.Location;
using GroceryList.Droid.Services;
using GroceryList.Droid.Services.Location;
using GroceryList.Droid.Services.Persistance.Impl;
using GroceryList.Service;
using GroceryList.Service.Locatization;
using GroceryList.Service.Persistance;
using Prism;
using Prism.Ioc;


namespace GroceryList.Droid
{
    [Activity(Label = "GroceryList", Icon = "@mipmap/ic_launcher", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static MainActivity Instance;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App(new AndroidInitializer()));

            Intent locationService = new Intent(this, typeof(LocationService));
            StartService(locationService);


        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            containerRegistry.Register<ISQLLiteDb, SQLiteDb>();
            containerRegistry.Register<INotification, NotificationHelper>();
            containerRegistry.Register<IMessage, MessageAndroid>();
        }
    }
}

