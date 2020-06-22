﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using GroceryList.Service;

namespace GroceryList.Droid.Services
{
    public class NotificationHelper : INotification
    {

        private Context mContext;
        private NotificationManager mNotificationManager;
        private NotificationCompat.Builder mBuilder;
        public static String NOTIFICATION_CHANNEL_ID = "10023";

        public NotificationHelper()
        {
            mContext = global::Android.App.Application.Context;
        }

        public void CreateNotification(string title, string message)
        {
            var alarmAttributes = new AudioAttributes.Builder()
                   .SetContentType(AudioContentType.Sonification)
                   .SetUsage(AudioUsageKind.Notification).Build();


            mBuilder = new NotificationCompat.Builder(mContext, NOTIFICATION_CHANNEL_ID);
            mBuilder.SetSmallIcon(Resource.Mipmap.icon);
            mBuilder.SetContentTitle(title)
                    .SetAutoCancel(true)
                    .SetContentTitle(title)
                    .SetContentText(message)
                    .SetPriority((int)NotificationPriority.High)
                    .SetVibrate(new long[0])
                    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                    .SetVisibility((int)NotificationVisibility.Public);


            NotificationManager notificationManager = mContext.GetSystemService(Context.NotificationService) as NotificationManager;

            if (global::Android.OS.Build.VERSION.SdkInt >= global::Android.OS.BuildVersionCodes.O)
            {
                NotificationImportance importance = global::Android.App.NotificationImportance.High;

                NotificationChannel notificationChannel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, title, importance);
                notificationChannel.EnableLights(true);
                notificationChannel.EnableVibration(true);
                notificationChannel.SetShowBadge(true);
                notificationChannel.Importance = NotificationImportance.High;
                notificationChannel.SetVibrationPattern(new long[] { 100, 200, 300, 400, 500, 400, 300, 200, 400 });

                if (notificationManager != null)
                {
                    mBuilder.SetChannelId(NOTIFICATION_CHANNEL_ID);
                    notificationManager.CreateNotificationChannel(notificationChannel);
                }
            }
            notificationManager.Notify(0, mBuilder.Build());
        }

    }
}