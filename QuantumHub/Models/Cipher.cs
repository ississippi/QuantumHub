using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumHub.Models
{
    public class Cipher
    {
        public int UserId { get; set; }
        public string SerialNumber { get; set; }
        public int StartingPoint { get; set; }
        public string CipherString { get; set; }
    }
}
