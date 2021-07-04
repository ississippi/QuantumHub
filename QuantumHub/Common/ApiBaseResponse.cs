using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuantumHub.Common
{
    public class BaseResponse<T>
    {
        public string status { get; set; }
        public string reason { get; set; }
        
        public T Data { get; set; }

    }
}
