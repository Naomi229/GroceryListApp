using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryList.Service
{
    public interface IMessage
    {
            void LongAlert(string message);
            void ShortAlert(string message);
    }
}
