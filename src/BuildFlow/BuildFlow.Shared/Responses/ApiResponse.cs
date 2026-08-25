using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildFlow.Shared.Responses
{
    public class ApiResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
