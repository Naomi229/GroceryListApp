using GroceryList.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GroceryList.Service
{
    public interface IGroceryService
    {

        ProductsList AddNewProductList(string name);
        Task<List<ProductsList>> GetAllProductsListAsync();

        Task<Product> AddNewProduct(string name, int quantity, Location location, bool bought, string listId);

        Task<List<Product>> GetAllProductFromListAsync(string listId);

    }
}
