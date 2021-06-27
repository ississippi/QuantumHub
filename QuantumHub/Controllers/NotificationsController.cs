using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuantumHub.Models;
using QuantumHub.Repository;

namespace QuantumHub.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        #region Public Methods
        // POST api/<CipherController>/GetNewCipher
        [HttpPost]
        public NotificationList GetNotifications([FromBody] int userId)
        {
            return null;
        }

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public Response SaveNotificationStatus([FromBody] Notification status)
        {
            return new Response { status = "success", reason = "" };
        }

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public void LongPollingNotifications([FromBody] int userId)
        {
        }

        #endregion Public Methods
        }
    }
