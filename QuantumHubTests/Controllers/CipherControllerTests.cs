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
        [TestMethod()]
        public void GetNewCipherTest()
        {
            var requestedLen = 1024;
            var serialLen = 75;
            var versionLen = 2;
            var prefixLen = serialLen + versionLen;
            var cipherReq = new NewCipherRequest
            {
                UserId = 1,
                Length = requestedLen
            };
            var ctl = new CipherController();
            var cipher = ctl.GetNewCipher(cipherReq);

            Assert.IsNotNull(cipher);
            Assert.IsTrue(cipher.CipherString.Length == requestedLen + prefixLen);
        }

        [TestMethod()]
        public void GetCipherListTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void GetCipherTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void UploadCipherTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void AcceptDenyCipherTest()
        {
            Assert.Fail();
        }
    }
}