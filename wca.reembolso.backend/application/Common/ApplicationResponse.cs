using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace application.Common
{
    public sealed class ApplicationResponse
    {
        public ApplicationResponse(bool isSuccess, string message, object? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public bool IsSuccess { get; set; } = true;
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

            
    }
}
