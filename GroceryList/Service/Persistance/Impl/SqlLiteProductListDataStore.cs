using GroceryList.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Service.Persistance.Impl
{
    public class SqlLiteProductListDataStore : BaseDataStore<ProductsList>, IProductListDataStore
    {

        public SqlLiteProductListDataStore(ISQLLiteDb db) : base(db.GetConnection())
        {
        }
    }
}
