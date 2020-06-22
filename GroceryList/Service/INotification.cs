using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryList.Service
{
    public interface INotification
    {
        void CreateNotification(String title, String message);
    }
}
