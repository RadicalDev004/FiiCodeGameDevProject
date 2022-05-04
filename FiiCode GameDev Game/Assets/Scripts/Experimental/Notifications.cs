using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications.Android;

public class Notifications : MonoBehaviour
{
    public AndroidNotificationChannel defaultNotificationChannel;
    int identifier;

    void Start()
    {
        AndroidNotificationCenter.CancelAllNotifications();
        ScheduleNotification(4, identifier);
    }

    public void ScheduleNotification(int hours, int identifier)
    {
        defaultNotificationChannel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default_Channel",
            Importance = Importance.Default,
            Description = "For generic notifications",

        };

        AndroidNotificationCenter.RegisterNotificationChannel(defaultNotificationChannel);

        AndroidNotification notification = new AndroidNotification()
        {
            Title = "Moon: Puzzles",
            Text = "Stars don't collect themselves! Don't forget to play today!",
            SmallIcon = "app_icon_small",
            LargeIcon = "app_icon_large",
            FireTime = System.DateTime.Now.AddHours(hours),
        };

        identifier = AndroidNotificationCenter.SendNotification(notification, "default_channel");

        AndroidNotificationCenter.NotificationReceivedCallback recievedNotificationHandler = delegate (AndroidNotificationIntentData data)
        {
            var msg = "Notification recieved : " + data.Id + "\n";
            msg += "\n Notification recieved: ";
            msg += "\n .Title" + data.Notification.Title;
            msg += "\n .Body" + data.Notification.Text;
            msg += "\n .Channel" + data.Channel;
            Debug.Log(msg);
        };

        AndroidNotificationCenter.OnNotificationReceived += recievedNotificationHandler;
        var notificationIntendData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (notificationIntendData != null)
        {
            Debug.Log("app was opened with notification");
        }
        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            AndroidNotificationCenter.CancelAllNotifications();
            AndroidNotificationCenter.SendNotification(notification, "default_channel");
        }
    }
}
