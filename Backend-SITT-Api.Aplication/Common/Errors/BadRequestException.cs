using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Common.Errors
{
    public class BadRequestException : ApplicationException
    {
        public BadRequestException(string? message) : base(message)
        {
        }
    }
}
