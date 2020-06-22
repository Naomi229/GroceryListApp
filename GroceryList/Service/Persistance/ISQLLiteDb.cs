using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryList.Service.Persistance
{
    public interface ISQLLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
