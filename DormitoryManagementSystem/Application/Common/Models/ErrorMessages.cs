namespace Application.Common.Models
{
	public static class ErrorMessages
	{
		public const string Invalid = nameof(Invalid);
		public const string InvalidEmail = nameof(InvalidEmail);
		public const string Required = nameof(Required);
		public const string MinLength = nameof(MinLength);
		public const string MaxLength = nameof(MaxLength);
		public const string MustContainLetter = nameof(MustContainLetter);

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

		public const string StartDateMustOccurBeforeEndDate = nameof(StartDateMustOccurBeforeEndDate);
		public const string MustBeInTheFuture = nameof(MustBeInTheFuture);
		public const string DateRangeOverlapsWithExisingRequest = nameof(DateRangeOverlapsWithExisingRequest);

		public const string RoomMustBeAvailable = nameof(RoomMustBeAvailable);
		public const string OfficeMustBeAvailable = nameof(OfficeMustBeAvailable);

		public const string RepairAlreadyRequested = nameof(RepairAlreadyRequested);
		public const string CannotModifyFixed = nameof(CannotModifyFixed);
	}
}
