using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using QuantumHub.Common;
using QuantumHub.Models;
using QuantumHub.Repository;
using Microsoft.AspNetCore.Http;

namespace QuantumHub.Controllers
{
  
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        #region Public Methods
        // POST api/<CipherController>/GetNotifications
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<CipherSendList>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetNotifications([FromBody] int userId)
        {
            // 1. Validate Request
            if (userId < 1)
            {
                return BadRequest(new BaseResponse<Cipher> { status = "fail", reason = "Invalid input UserId.", Data = null });
            }
            // 2. Pull Notifications from the DB for this user.
            var c = NotificationsRepository.GetNotifications(userId);
            // 3. Return to Caller
            return Ok(new BaseResponse<CipherSendList> { status = "success", reason = "", Data = c });
        }


        // POST api/<CipherController>/WebSocketsNotifications
        [HttpPost]
        public void WebSocketsNotifications([FromBody] int userId)
        {
        }

        #endregion Public Methods
    }
}
