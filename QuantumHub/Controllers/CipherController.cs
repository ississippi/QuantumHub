using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using QuantumHub.Models;
using QuantumHub.Repository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuantumHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CipherController : ControllerBase
    {
        readonly string _cipherVersion = "10";

        #region Public Methods

        // POST api/<CipherController>/GetNewCipher
        [HttpPost]
        public Cipher GetNewCipher([FromBody] NewCipherRequest newCipherReq)
        {
            // 1. Generate Cipher
            var newCipherString = GetRandomCipher(newCipherReq.UserId, newCipherReq.Length);
            // 2. Save to DB
            var cl = CipherRepository.SaveCipher(newCipherString);
            // 3. Return to requestor
            return newCipherString;
        }

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public CipherList GetCipherList([FromBody] int userId)
        {
            // 1. Get Ciphers from DB for user
            var cl = CipherRepository.GetCipherListByUser(userId);
            // 2. Return to caller
            return cl;
        }

        // POST api/<CipherController>/GetCipher
        [HttpPost]
        public Cipher GetCipher([FromBody] CipherRequest request)
        {
            // 1. Pull cipher from the DB by Input parameters
            // 2. Return to Caller
            return null;
        }
        // POST api/<CipherController>/UploadCipher
        [HttpPost]
        public Response UploadCipher([FromBody] Cipher cipher)
        {
            // 1. Validate Cipher input parameters
            // 2. Store Cipher into DB
            return new Response { status = "success", reason = "" };
        }

        // POST api/<CipherController>/AcceptDenyCipher
        [HttpPost]
        public Response AcceptDenyCipher([FromBody] CipherAcceptDeny acceptDeny)
        {
            return new Response { status = "success", reason = "" };
        }

        #endregion Public Methods

        #region Private Functions

        private Cipher GetRandomCipher(int userId, int cipherLen)
        {
            var version = "01";
            var serialNo = GenerateRandomSerialNumber();
            var newCipher = GenerateRandomCryptographicKey(cipherLen);
            var cipher = new Cipher
            {
                CipherString = version + serialNo + newCipher,
                SerialNumber = serialNo,
                StartingPoint = 0,
                UserId = userId
            };
            return cipher;
        }

        private static string GenerateRandomCryptographicKey(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new();
            byte[] randomBytes = new byte[keyLength];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes).Substring(0, keyLength);
            //return Encoding.Default.GetString(randomBytes);
        }
        private static string GenerateRandomSerialNumber()
        {
            var cipherRaw = Guid.NewGuid().ToString();
            var cipherStripped = Regex.Replace(cipherRaw, @"\-", "");
            var cipher = cipherStripped;
            cipherRaw = Guid.NewGuid().ToString();
            cipherStripped = Regex.Replace(cipherRaw, @"\-", "");
            cipher += cipherStripped;
            cipherRaw = Guid.NewGuid().ToString();
            cipherStripped = Regex.Replace(cipherRaw, @"\-", "");
            cipher += cipherStripped;
            cipher = cipher.Substring(cipher.Length - 75, 75);
            //var cipher = string.Format("{0, 75}", cipherStripped);
            return cipher;
        }


        #endregion Private Functions
    }
}
