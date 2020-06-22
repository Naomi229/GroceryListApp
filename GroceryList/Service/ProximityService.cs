using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GroceryList.Service
{
    public class ProximityService
    {

        private static readonly double MIN_DISTANCE = 1.0;

        private readonly GroceryService groceryService;
        private readonly INotification notification;

        public ProximityService(GroceryService groceryService, INotification notification)
        {
            this.groceryService = groceryService;
            this.notification = notification;
        }



        public async Task CheckProximityAsync(double currentLatitude, double currentLongitude)
        {
            var products = await groceryService.GetProductsNotBought();

            foreach (var prod in products)
            {
                double distance = calculateDistance(currentLatitude, currentLongitude, prod.Lat, prod.Lon);
                if ( distance <= MIN_DISTANCE)
                {
                    string listName = await groceryService.GetListNameById(prod.ListId);
                    notification.CreateNotification("GroceryList", $"List: {listName}; {string.Format("{0:N2}", distance)}km distance from product: {prod.Name}");
                }
            }
        }


        private double calculateDistance(double currenLat, double currentLong, double prodLat, double prodLong)
        {
            return Location.CalculateDistance(currenLat, currentLong, prodLat, prodLong, DistanceUnits.Kilometers);

        }
    }
}
