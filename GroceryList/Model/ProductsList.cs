using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryList.Model
{
    public class ProductsList
    {
        [PrimaryKey]
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Completed { get; set; }
        public string DateCreated { get; set; }
        [Ignore]
        public int NumberOfItmes { get; set; }


    }
}
