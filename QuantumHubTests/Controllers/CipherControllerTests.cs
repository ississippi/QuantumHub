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
    public class CipherControllerTests
    {
        int SERIALNO_LEN = 75;
        int VERSION_LEN = 2;
        int CIPHER_PREFIX_LEN = 77;
        [TestMethod()]
        public void GetNewCipherTest()
        {
            var requestedLen = 1024;
            var cipherReq = new NewCipherRequest
            {
                UserId = 1,
                Length = requestedLen
            };
            var ctl = new CipherController();
            var response = ctl.GetNewCipher(cipherReq);

            Assert.IsNotNull(response);
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<Cipher>)okResult.Value;
            var c = br.Data;
            Assert.IsNotNull(c);
            Assert.IsTrue(c.CipherString.Length == requestedLen + CIPHER_PREFIX_LEN);
        }

        [TestMethod()]
        public void GetNewCipher_Small_Test()
        {
            var requestedLen = 96;
            var cipherReq = new NewCipherRequest
            {
                UserId = 1,
                Length = requestedLen
            };
            var ctl = new CipherController();
            var response = ctl.GetNewCipher(cipherReq);

            Assert.IsNotNull(response);
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<Cipher>)okResult.Value;
            var c = br.Data;
            Assert.IsNotNull(c);
            Assert.IsTrue(c.CipherString.Length == requestedLen + CIPHER_PREFIX_LEN);
        }

        [TestMethod()]
        public void GetCipherListTest()
        {
            var userId = 1;
            var ctl = new CipherController();
            var response = ctl.GetCipherList(userId);

            Assert.IsNotNull(response);
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<CipherList>)okResult.Value;
            var c = br.Data;
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Ciphers.Count > 0);

        }

        [TestMethod()]
        public void GetCipherTest()
        {
            var request = new CipherRequest
            {
                UserId = 1,
                //SerialNumber = "f642bfb037f9bd1227a8fc6435ba211fb922d544cd75f9f72cc8ddf4f77a6d84d1e0bdb7c09"
                CipherId = 3
            };
            var ctl = new CipherController();
            var response = ctl.GetCipher(request);

            Assert.IsNotNull(response);
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<Cipher>)okResult.Value;
            var c = br.Data;
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.CipherString);
            Assert.IsTrue(c.CipherId > 0);
            Assert.IsTrue(c.CreatedDateTime > DateTime.MinValue);
            Assert.IsTrue(c.StartingPoint > -1);
            Assert.IsFalse(string.IsNullOrEmpty(c.SerialNumber));
            Assert.IsTrue(c.SerialNumber.Length == SERIALNO_LEN);
            Assert.IsFalse(string.IsNullOrEmpty(c.CipherString));
            Assert.IsTrue(c.CipherString.Length > CIPHER_PREFIX_LEN);
        }

        [TestMethod()]
        public void SendCipherTest()
        {
            var request = new CipherRequest
            {
                UserId = 1,
                //SerialNumber = "34da8aa02a4d1e8cf4495724e1aa5ab05894da2457f35972ba943ce45a68a31b29fad4aff9a"
                CipherId = 4
            };
            var ctl = new CipherController();
            var cipherResponse = ctl.GetCipher(request);
            Assert.IsNotNull(cipherResponse);
            var okResult = cipherResponse as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<Cipher>)okResult.Value;
            var c = br.Data;

            var send = new CipherSend
            {
                SenderUserId = 1,
                RecipientUserId = 3,
                CipherId = 4,
                StartingPoint = c.StartingPoint
            };
            var response = ctl.SendCipher(send);

            Assert.IsNotNull(response);
            okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var sendIdValue = (BaseResponse<int>)okResult.Value;
            var sendId = sendIdValue.Data;
            //Assert.IsTrue(sendId > 0); //TODO: need to validate return sendId
        }

        [TestMethod()]
        public void AcceptDeny_Accept_SuccessTest()
        {
            var cad = new CipherAcceptDeny
            {
                AcceptDeny = "Accept",
                CipherSendRequestId = 1
            };
            var ctl = new CipherController();
            var response = ctl.AcceptDenyCipher(cad);

            Assert.IsNotNull(response);
            var okResult = response as OkObjectResult;
            Assert.IsNotNull(okResult.Value);
            var br = (BaseResponse<Cipher>)okResult.Value;
            var c = br.Data;
            Assert.IsNotNull(c);
            Assert.IsTrue(c.CipherString.Length > CIPHER_PREFIX_LEN);
        }
    }
}