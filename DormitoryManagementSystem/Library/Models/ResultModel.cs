using System.Collections.Generic;

namespace Library.Models
{
    public class ResultModel
    {
        public bool Success { get; set; }

        public bool Fail => !Success;

        public string ErrorMessage { get; set; }

        public List<ErrorDetail> ErrorDetails { get; set; }

        public static ResultModel Successful => new ResultModel { Success = true };

        public class ErrorDetail
        {
            public string PropertyName { get; set; }

            public string Message { get; set; }
        }
    }
}
