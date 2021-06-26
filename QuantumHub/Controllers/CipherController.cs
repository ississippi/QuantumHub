using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using QuantumHub.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuantumHub.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class CipherController : ControllerBase
    {
        #region Public Methods

        // POST api/<CipherController>/GetNewCipher
        [HttpPost]
        public Cipher GetNewCipher([FromBody] string value)
        {
            return null;
        }

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public void GetCipherList([FromBody] string value)
        {
        }

        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public void GetCipher([FromBody] string value)
        {
        }
        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public void UploadCipher([FromBody] string value)
        {
        }
        // POST api/<CipherController>/GetCipherList
        [HttpPost]
        public void AcceptDenyCipher([FromBody] string value)
        {
        }

        #endregion Public Methods

        #region Private Functions


        #endregion Private Functions
    }
}
