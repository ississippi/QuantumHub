using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuantumHub.Models;

namespace QuantumHub.Repository
{
    public static class NotificationsRepository
    {
        #region Public Methods

        public static NotificationList GetNotifications(int userId)
        {

            return null;
        }
        public static int SaveNotification(Notification notif)
        {
            var notifId = -1;

            return notifId;
        }
        public static bool UpdateNotificationsStatus(NotificationList notifs)
        {

            return false;
        }

        #endregion Public Methods

        #region Private Functions


        #endregion Private Functions
    }
}
