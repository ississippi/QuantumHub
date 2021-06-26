using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumHub.Models
{
    public class Notification
    {
        public int Status { get; set; }
        public int UserId { get; set; }
        public int FromUserId { get; set; }
        public int NotificationId { get; set; }
        public int NotificationText { get; set; }
    }

    public class NotificationList
    {
        public List<Notification> Notifications { get; set; }
        public int Status { get; set; }
    }
}
