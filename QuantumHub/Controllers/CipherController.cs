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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuantumHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CipherController : ControllerBase
    {
        readonly string _cipherVersion = "10";

        #region Public Methods

        [HttpGet]
        [Route("Health")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public void Health ()
        {
        }

        // POST api/<CipherController>/GetNewCipher
        [HttpPost]
        [Route("GetNewCipher")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<Cipher>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetNewCipher([FromBody] NewCipherRequest newCipherReq)
        {
            // 1. Generate Cipher
            var newCipher = GetRandomCipher(newCipherReq.UserId, newCipherReq.Length);
            // 2. Save to DB
            var cid = CipherRepository.SaveCipher(newCipher);
            newCipher.CipherId = cid;
            // 3. Return to requestor
            return Ok(new BaseResponse<Cipher> { status = "success", reason = "", Data = newCipher });
        }

        /// <summary>
        /// POST api/<CipherController>/CipherSerialRequest
        /// Get a list of globally unique serial numbers.
        /// NOTE: While still in demo mode, no serial numbers are persisted to the DB, thought the Repository functions, tables and stored procedures have been built.
        /// A list of serial numbers will be created and saved to the database and associated with the requesting user.
        /// The serial numbers will be marked inactivated until they have been saved to a cipher segment file after being used to encrypt.
        /// When the user requests another set of ciphers, the former set of inactive serial numbers are discarded and replaced with the newly generated serial numbers.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CipherSerialRequest")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<CipherSerials>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetNewCipherSerialsForUser([FromBody] CipherSerialRequest request)
        {
            // 1. Generate a list of ciphers per user requested quantity.
            var serials = new CipherSerials();
            for (var i = 0; i < request.Quantity; i++)
            {
                serials.SerialNumbers.Add(GenerateRandomSerialNumber());
            }

            return Ok(new BaseResponse<CipherSerials> { status = "success", reason = "", Data = serials });
        }

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        [Route("GetCipherList")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<CipherList>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(BaseResponse<string>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(BaseResponse<string>))]
        public IActionResult GetCipherList([FromBody] int userId)
        {
            try {
                // 1. Get Ciphers from DB for user
                var cl = CipherRepository.GetCipherListByUser(userId);
                // 2. Return to caller
                return Ok(new BaseResponse<CipherList> { status = "success", reason = "", Data = cl });
            }
            catch (Exception e)
            {
                return BadRequest(new BaseResponse<string> { status = "fail", reason = e.Message, Data = e.StackTrace + " connectionString: " + CipherRepository.getConnectionString() });
            }
        }

        // POST api/<CipherController>/GetCipher
        [HttpPost]
        [Route("GetCipher")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<Cipher>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCipher([FromBody] CipherRequest request)
        {
            // 1. Validate Request
            if (request == null || request.UserId < 1 || request.CipherId < 1)
            {
                return BadRequest(new BaseResponse<Cipher> { status = "fail", reason = "Invalid input UserId or SerialNumber.", Data = null });
            }
            // 2. Pull cipher from the DB by Input parameters
            var c = CipherRepository.GetCipher(request.UserId, request.CipherId);
            // 3. Return to Caller
            return Ok(new BaseResponse<Cipher> { status = "success", reason = "", Data = c });
        }

        // POST api/<CipherController>/SendCipher
        [HttpPost]
        [Route("SendCipher")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SendCipher([FromBody] CipherSend s)
        {
            // 1. Validate Cipher input parameters
            if (s == null ||  s.SenderUserId < 1 || s.RecipientUserId < 1 || s.CipherId < 1 || s.StartingPoint < 0)
            {
                throw new Exception("Bad Request");
            }
            // 2. Save to DB
            var cipherSendId = CipherRepository.SendCipher(s);
            // 3. Return to requestor
            return Ok(new BaseResponse<int> { status = "success", reason = "", Data = cipherSendId});
        }

        // POST api/<CipherController>/UploadCipher
        [HttpPost]
        [Route("UploadCipher")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<int>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UploadCipher([FromBody] CipherUpload u)
        {
            // 1. Validate Cipher input parameters
            if (u == null || u.UserId < 1 || u.CipherObj == null || u.CipherObj.UserId < 1 || string.IsNullOrEmpty(u.CipherObj.CipherString) || string.IsNullOrEmpty(u.CipherObj.SerialNumber))
            {
                throw new Exception("Bad Request");
            }
            // 2. Save to DB
            var cipherId = CipherRepository.SaveCipher(u.CipherObj);
            // 3. Return to requestor
            return Ok(new BaseResponse<int> { status = "success", reason = "", Data = cipherId });
        }

        // POST api/<CipherController>/AcceptDenyCipher
        [HttpPost]
        [Route("AcceptDenyCipher")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<Cipher>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AcceptDenyCipher([FromBody] CipherAcceptDeny acceptDeny)
        {
            // 1. Validate Input
            // If Accept, save status and return cipher
            if (acceptDeny.AcceptDeny.ToLower() == "accept")
            {
                // 1. Save status
                var cipherId = CipherRepository.SaveSendCipherStatus(acceptDeny);
                // 2. Get Cipher
                var c = CipherRepository.GetCipherFromSend(acceptDeny.CipherSendRequestId);
                c.UserId = acceptDeny.UserId;
                // 2. Save to DB
                var cid = CipherRepository.SaveCipher(c);

                return Ok(new BaseResponse<Cipher> { status = "success", reason = "", Data = c});
            }
            // If Deny, save response and notify sender.
            else
            {
                // 1. Save status and all done.
                var cipherId = CipherRepository.SaveSendCipherStatus(acceptDeny);

            }
            return Ok(new BaseResponse<Cipher> { status = "success", reason = "" });
        }

        #endregion Public Methods

        #region Private Functions

        private Cipher GetRandomCipher(int userId, int cipherLen)
        {
            var serialNo = GenerateRandomSerialNumber();
            var newCipher = GenerateRandomCryptographicKey(cipherLen);
            var maxEncrypt = cipherLen; // QuantumEncrypt.GetMaxFileSizeForEncryption(newCipher);
            var cipher = new Cipher
            {
                CipherString = _cipherVersion + serialNo + newCipher,
                MaxEncryptionLength = maxEncrypt,
                SerialNumber = serialNo,
                StartingPoint = 0,
                UserId = userId
            };
            return cipher;
        }

        private static string GenerateRandomCryptographicKey(int keyLength)
        {
            RNGCryptoServiceProvider rngCryptoServiceProvider = new RNGCryptoServiceProvider();
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
