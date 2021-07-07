using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using QuantumHub.Common;
using QuantumHub.Controllers;
using QuantumHub.Models;

namespace QuantumHub.Controllers.Tests
{
    [TestClass()]
    public class NotificationsControllerTests
    {
        [TestMethod()]
        public void GetNotificationsTest()
        {
            var recipientUserId = 3;
            var ctl = new NotificationsController();
            var response = ctl.GetNotifications(recipientUserId);

            Assert.IsNotNull(response);
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<CipherSendList>)okResult.Value;
            var n = br.Data;
            Assert.IsNotNull(n);
            Assert.IsTrue(n.SendRequests.Count > 0);
        }

        [TestMethod()]
        public void NotificationsWebSocketsTest()
        {
            Assert.Fail();
        }
    }
}