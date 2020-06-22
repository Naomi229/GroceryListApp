using GroceryList.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GroceryList.Service.Persistance.Impl
{
    // Sql lite implemntation for IDataStore<Product>
    public class SqlLiteProductDataStore : BaseDataStore<Product>, IProdcutDataStore
    {

        public SqlLiteProductDataStore(ISQLLiteDb db) : base(db.GetConnection())
        {
           
        }
    }
}
