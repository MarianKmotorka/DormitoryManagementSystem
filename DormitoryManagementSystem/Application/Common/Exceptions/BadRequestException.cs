using System;
using System.Collections.Generic;

namespace Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }

        public BadRequestException(IEnumerable<string> messages)
            : base(string.Join($"{Environment.NewLine}", messages))
        {
        }
    }
}
