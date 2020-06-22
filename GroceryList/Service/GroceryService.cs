using GroceryList.Model;
using GroceryList.Service.Persistance;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GroceryList.Service
{
    public class GroceryService : IGroceryService
    {
        private readonly IProdcutDataStore productDataStore;
        private readonly IProductListDataStore prodcutListDataStore;

        public GroceryService(IProdcutDataStore productDataStore, IProductListDataStore prodcutListDataStore)
        {
            this.productDataStore = productDataStore;
            this.prodcutListDataStore = prodcutListDataStore;
        }

        public ProductsList AddNewProductList(string name)
        {
            ProductsList pl = new ProductsList {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Completed = false,
                DateCreated = DateTime.Now.ToString()
            };
            prodcutListDataStore.AddAsync(pl);
            return pl;
        }
       
        public async Task<List<ProductsList>> GetAllProductsListAsync()
        {
            List<ProductsList> lists = new List<ProductsList>();
                try
                {
                    var items = await prodcutListDataStore.GetAllAsync(true);

                    foreach (var item in items)
                    {
                        int numberOfItemsFromList = await productDataStore.ExecuteScalarAsync(String.Format("Select Count(*) from Product where ListId='{0}'", item.Id));
                        int numberOfUnBoughtProd = await productDataStore.ExecuteScalarAsync(String.Format("Select Count (*) from Product where ListId='{0}' and Bought={1}", item.Id, false));

                        item.NumberOfItmes = numberOfItemsFromList;
                        if (numberOfUnBoughtProd == 0 && numberOfItemsFromList != 0)
                        {
                            item.Completed = true;
                        }

                        lists.Add(item);
                        Debug.WriteLine("List: id:" + item.Id + "Name: " + item.Name);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }
            return lists;
        }

        public async Task<Product> AddNewProduct(string name, int quantity, Location location, bool bought, string listId)
        {
            Product p = new Product { Id = Guid.NewGuid().ToString(), Name = name, Quantity = quantity, Lat = location.Latitude, Lon = location.Longitude, Bought = bought, ListId = listId };

            await productDataStore.AddAsync(p);
            return p;
        }

        public async Task ChangeProductBoughtStatus(Product product, bool isBought)
        {
            product.Bought = isBought;
            await productDataStore.UpdateAsync(product);
            Debug.WriteLine($"{product.Id} was updated");
        }

        public async Task<List<Product>> GetAllProductFromListAsync(string listId)
        {

            /*List<Product> products = new List<Product>();

            try
            {
                var items = await productDataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    if (item.ListId.Equals(listId))
                    {
                        products.Add(item);
                        
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return products;
            */


           return await productDataStore.ExecuteQueryAsync("Select * from Product where ListId=?", listId);
        }

        public async Task<string> GetListNameById(string listId)
        {
            var list =  await prodcutListDataStore.GetAsync(listId);
            return list.Name;
        }
        public async Task<IEnumerable<Product>> GetProductsNotBought()
        {

            var result = await productDataStore.ExecuteQueryAsync(String.Format("Select * from {0} where Bought={1}", "Product", false));

            Debug.WriteLine($"Found {result.Count()} products that are not bought");
            return result;
        }

        public async Task DeleteProduct(Product product)
        {
            bool result = await productDataStore.DeleteAsync(product);
            Debug.WriteLine($"Deleted product with id={product.Id} ? {result}");
        }

        public async Task DeleteProductsFromList(string listId)
        {
            var result = await productDataStore.ExecuteQueryAsync(String.Format("delete from {0} where ListId='{1}'", "Product", listId));
            Debug.WriteLine($"{result.Count} lines were deleted");
        }

        public async Task DeleteList(ProductsList productsList)
        {
            bool result = await prodcutListDataStore.DeleteAsync(productsList);
            Debug.WriteLine("delted produtList with id=" + productsList.Id + "? " + result);
           
            await DeleteProductsFromList(productsList.Id);
            Debug.WriteLine($"Deleted list and contetnt for listId={productsList.Id}");
        }
    }
}
