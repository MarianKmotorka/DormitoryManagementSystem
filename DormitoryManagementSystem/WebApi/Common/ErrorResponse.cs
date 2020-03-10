using System.Collections.Generic;

namespace WebApi.Common
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public List<ErrorDetail> ErrorDetails { get; set; } = new List<ErrorDetail>();
    }
}
