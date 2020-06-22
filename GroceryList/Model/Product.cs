using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace GroceryList.Model
{
    public class Product
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }
        public bool Bought { get; set; }
        public string ListId { get; set; }

    }
}
