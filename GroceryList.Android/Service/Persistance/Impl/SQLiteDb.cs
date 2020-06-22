using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GroceryList.Droid.Services.Persistance.Impl;
using GroceryList.Service.Persistance;
using SQLite;
using Xamarin.Forms;
using Environment = System.Environment;


//[assembly: Dependency(typeof(SQLiteDb))]

namespace GroceryList.Droid.Services.Persistance.Impl
{
    public class SQLiteDb : ISQLLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "GroceryListDataB.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}