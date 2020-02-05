using System;
using Application.Common.Models;
using FluentValidation;

namespace Application.AccomodationRequests.Commands.CreateAccomodationRequest
{
    public class CreateAccomodationRequestCommandValidator : AbstractValidator<CreateAccomodationRequestCommand>
    {
        public CreateAccomodationRequestCommandValidator()
        {
            RuleFor(x => x.AccomodationStartDateUtc)
                .Must((command, start) => start < command.AccomodationEndDateUtc)
                .WithMessage(ErrorMessages.StartDateMustOccurBeforeEndDate);

            RuleFor(x => x.AccomodationStartDateUtc)
                .Must(x => x > DateTime.UtcNow)
                .WithMessage(ErrorMessages.StartDateMustBeInTheFuture);

            //TODO add CreateAccomodationRequestCommandValidations..... cannot place another request when there already is a request for that time span
        }
    }
}
