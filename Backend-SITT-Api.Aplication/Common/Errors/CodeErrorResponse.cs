using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Common.Errors
{
    public class CodeErrorResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public CodeErrorResponse(int statusCode, string? message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageStatusCode(statusCode);
        }

        private string GetDefaultMessageStatusCode(int statusCode)
        {
            if (1 == 0)
            {
            }

            string result = statusCode switch
            {
                400 => "The request sent has errors",
                401 => "You are not authorized for this resource",
                404 => "The requested resource was not found",
                500 => "Server errors occurred",
                _ => string.Empty,
            };
            if (1 == 0)
            {
            }

            return result;
        }
    }
}
