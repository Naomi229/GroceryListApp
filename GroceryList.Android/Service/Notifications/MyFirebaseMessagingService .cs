using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.Messaging;
using GroceryList.Droid.Services;
using GroceryList.Service;

namespace GroceryList.Droid.Service.Notifications
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {

        const string TAG = "MyFirebaseMsgService";

        public override void OnMessageReceived(RemoteMessage message)
        {
            Log.Debug(TAG, "From: " + message.From);
            Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
            new NotificationHelper().CreateNotification(message.GetNotification().Title, message.GetNotification().Body);

        }
    }
}