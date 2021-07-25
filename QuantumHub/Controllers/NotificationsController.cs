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
        [HttpGet]
        [Route("Health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Health()
        {
        }
        
        // POST api/<CipherController>/GetNotifications
        [HttpPost]
        [Route("GetNotifications")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<CipherSendList>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse<string>))]
        public IActionResult GetNotifications([FromBody] NotificationRequest request)
        {
            try
            {
                // 1. Validate Request
                if (request.RecipientId < 1)
                {
                    return BadRequest(new BaseResponse<Cipher> { status = "fail", reason = "Invalid input UserId.", Data = null });
                }
                // 2. Pull Notifications from the DB for this user.
                var c = NotificationsRepository.GetNotifications(request);
                var nl = NotificationsRepository.AddMaxEncryptLengthToNotifications(ref c);
                // 3. Return to Caller
                return Ok(new BaseResponse<CipherSendList> { status = "success", reason = "", Data = nl });
            }
            catch(Exception e)
            {
                return BadRequest(new BaseResponse<string> { status = "fail", reason = e.Message, Data = e.StackTrace + " connectionString: " + CipherRepository.getConnectionString() });
            }


        }


        // POST api/<CipherController>/WebSocketsNotifications
        //[HttpPost]
        //public void WebSocketsNotifications([FromBody] int userId)
        //{
        //}

        #endregion Public Methods
    }
}
