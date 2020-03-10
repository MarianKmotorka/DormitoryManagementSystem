using System.Collections.Generic;
using FluentValidation.Results;

namespace WebApi.Common
{
    public class ErrorResponseUtil
    {
        public static ErrorResponse CreateBadRequestErrorResponse(string errorMessage)
        {
            return new ErrorResponse
            {
                ErrorCode = "BadRequest",
                ErrorMessage = errorMessage
            };
        }

        public static ErrorResponse CreateBadRequestErrorResponse(IEnumerable<ValidationFailure> validationFailures)
        {
            var response = new ErrorResponse
            {
                ErrorCode = "ValidationError",
                ErrorMessage = "Request validation failed"
            };

            foreach (var failure in validationFailures)
            {
                var errorDetail = new ErrorDetail
                {
                    Message = failure.ErrorMessage,
                    CustomState = failure.CustomState,
                    PropertyName = failure.PropertyName
                };

                response.ErrorDetails.Add(errorDetail);
            }

            return response;
        }

        public static ErrorResponse CreateNotFoundErrorResponse(string errorMessage = null)
        {
            if (errorMessage != null && string.IsNullOrWhiteSpace(errorMessage))
                errorMessage = null;

            return new ErrorResponse
            {
                ErrorCode = "NotFound",
                ErrorMessage = errorMessage ?? "Please check if you typed the url correctly."
            };
        }
    }
}
