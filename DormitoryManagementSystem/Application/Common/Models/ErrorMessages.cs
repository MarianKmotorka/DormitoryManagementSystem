﻿namespace Application.Common.Models
{
    public static class ErrorMessages
    {
        public const string Invalid = nameof(Invalid);

        public const string JwtIsNotExpired = nameof(JwtIsNotExpired);
        public const string RefreshTokenDoesNotExist = nameof(RefreshTokenDoesNotExist);
        public const string RefreshTokenExpired = nameof(RefreshTokenExpired);
        public const string RefreshTokenAlreadyUsed = nameof(RefreshTokenAlreadyUsed);
        public const string RefreshTokenDoesNotMatchJwt = nameof(RefreshTokenDoesNotMatchJwt);

        public const string AlreadyConfirmed = nameof(AlreadyConfirmed);
        public const string EmailNotFound = nameof(EmailNotFound);
        public const string EmailNotUnique = nameof(EmailNotUnique);
        public const string InvalidEmailOrPassword = nameof(InvalidEmailOrPassword);
        public const string EmailNotConfirmed = nameof(EmailNotConfirmed);
    }
}