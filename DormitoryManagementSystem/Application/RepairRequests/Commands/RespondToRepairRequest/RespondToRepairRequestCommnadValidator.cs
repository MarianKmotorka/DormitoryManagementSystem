using System;
using Application.Common.Models;
using Domain.Enums;
using FluentValidation;

namespace Application.RepairRequests.Commands.RespondToRepairRequest
{
    public class RespondToRepairRequestCommnadValidator : AbstractValidator<RespondToRepairRequestCommand>
    {
        public RespondToRepairRequestCommnadValidator()
        {
            RuleFor(x => x.RepairerReply).Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ErrorMessages.Required).When(x => x.RepairRequestState == RepairRequestState.Refused)
                .MinimumLength(5).WithMessage(ErrorMessages.MinLength).WithState(_ => 5).When(x => x.RepairRequestState == RepairRequestState.Refused);

            RuleFor(x => x.WillBeFixedOn).Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull().WithMessage(ErrorMessages.Required).When(x => x.RepairRequestState == RepairRequestState.Accepted)
                .Must(x => x > DateTime.UtcNow).WithMessage(ErrorMessages.MustBeInTheFuture).When(x => x.RepairRequestState == RepairRequestState.Accepted);

            RuleFor(x => x.RepairRequestState)
                .Must(x => x != RepairRequestState.Pending).WithMessage(ErrorMessages.Invalid);
        }
    }
}
