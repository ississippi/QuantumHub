using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantumHub.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var cipher = ctl.GetNewCipher(cipherReq);

            Assert.IsNotNull(cipher);
            Assert.IsTrue(cipher.CipherString.Length == requestedLen + CIPHER_PREFIX_LEN);
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
            var cipher = ctl.GetNewCipher(cipherReq);

            Assert.IsNotNull(cipher);
            Assert.IsTrue(cipher.CipherString.Length == requestedLen + CIPHER_PREFIX_LEN);
        }

        [TestMethod()]
        public void GetCipherListTest()
        {
            var userId = 1;
            var ctl = new CipherController();
            var ciphers = ctl.GetCipherList(userId);
            Assert.IsNotNull(ciphers);
            Assert.IsTrue(ciphers.Ciphers.Count > 0);
        }

        [TestMethod()]
        public void GetCipherTest()
        {
            var request = new CipherRequest
            {
                UserId = 1,
                SerialNumber = "f642bfb037f9bd1227a8fc6435ba211fb922d544cd75f9f72cc8ddf4f77a6d84d1e0bdb7c09"
            };
            var ctl = new CipherController();
            var cipher = ctl.GetCipher(request);
            Assert.IsNotNull(cipher);
            Assert.IsTrue(cipher.CipherId > 0);
            Assert.IsTrue(cipher.CreatedDateTime > DateTime.MinValue);
            Assert.IsTrue(cipher.StartingPoint > -1);
            Assert.IsFalse(string.IsNullOrEmpty(cipher.SerialNumber));
            Assert.IsTrue(cipher.SerialNumber.Length == SERIALNO_LEN);
            Assert.IsFalse(string.IsNullOrEmpty(cipher.CipherString));
            Assert.IsTrue(cipher.CipherString.Length > CIPHER_PREFIX_LEN);
        }

        [TestMethod()]
        public void SendCipherTest()
        {
            var request = new CipherRequest
            {
                UserId = 1,
                SerialNumber = "34da8aa02a4d1e8cf4495724e1aa5ab05894da2457f35972ba943ce45a68a31b29fad4aff9a"
            };
            var ctl = new CipherController();
            var cipher = ctl.GetCipher(request);
            var send = new CipherSend
            {
                UserId = 1,
                RecipientUserId = 3,
                CipherId = 4,
                StartingPoint = cipher.StartingPoint
            };
            var response = ctl.SendCipher(send);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.status == "success");
        }

        [TestMethod()]
        public void AcceptDenyCipherTest()
        {
            Assert.Fail();
        }
    }
}