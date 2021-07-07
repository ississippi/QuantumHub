﻿using Microsoft.AspNetCore.Mvc;
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

        // POST api/<CipherController>/GetNewCipher
        [HttpPost]
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

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<CipherList>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCipherList([FromBody] int userId)
        {
            // 1. Get Ciphers from DB for user
            var cl = CipherRepository.GetCipherListByUser(userId);
            // 2. Return to caller
            return Ok(new BaseResponse<CipherList> { status = "success", reason = "", Data = cl });
        }

        // POST api/<CipherController>/GetCipher
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<Cipher>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCipher([FromBody] CipherRequest request)
        {
            // 1. Validate Request
            if (request == null || request.UserId < 1 || string.IsNullOrEmpty(request.SerialNumber) || request.SerialNumber.Length != 75)
            {
                return BadRequest(new BaseResponse<Cipher> { status = "fail", reason = "Invalid input UserId or SerialNumber.", Data = null });
            }
            // 2. Pull cipher from the DB by Input parameters
            var c = CipherRepository.GetCipher(0, request.UserId, request.SerialNumber);
            // 3. Return to Caller
            return Ok(new BaseResponse<Cipher> { status = "success", reason = "", Data = c });
        }

        // POST api/<CipherController>/UploadCipher
        [HttpPost]
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

        // POST api/<CipherController>/AcceptDenyCipher
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BaseResponse<Cipher>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AcceptDenyCipher([FromBody] CipherAcceptDeny acceptDeny)
        {
            // 1. Validate Input
            // If Accept, save status and return cipher
            if (acceptDeny.AcceptDeny.ToLower() == "accept")
            {
                //    Save status
                var cipherId = CipherRepository.SaveSendCipherStatus(acceptDeny);
                //    Get and return Cipher
                var c = CipherRepository.GetCipherFromSend(acceptDeny.CipherSendRequestId);
                return Ok(new BaseResponse<Cipher> { status = "success", reason = "", Data = c});
            }
            // If Deny, save response and notify sender.
            else
            {
                //return Ok(new BaseResponse<Cipher> { status = "success", reason = "" });

            }
            return Ok(new BaseResponse<Cipher> { status = "success", reason = "" });
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
