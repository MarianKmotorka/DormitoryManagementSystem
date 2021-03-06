﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.AccomodationRequests.Commands.CreateAccomodationRequest
{
    public class CreateAccomodationRequestCommandValidator : AbstractValidator<CreateAccomodationRequestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public CreateAccomodationRequestCommandValidator(IDormitoryDbContext db) : this()
        {
            _db = db;
        }

        private CreateAccomodationRequestCommandValidator()
        {
            RuleFor(x => x.AccomodationStartDateUtc).Cascade(CascadeMode.StopOnFirstFailure)
                .Must((command, start) => start < command.AccomodationEndDateUtc)
                .WithMessage(ErrorMessages.StartDateMustOccurBeforeEndDate)
                .Must(x => x > DateTime.UtcNow)
                .WithMessage(ErrorMessages.MustBeInTheFuture)
                .MustAsync(NotOverlap).WithMessage(ErrorMessages.DateRangeOverlapsWithExisingRequest);

        }

        private async Task<bool> NotOverlap(CreateAccomodationRequestCommand command, DateTime start, CancellationToken cancellationToken)
        {
            var requests = await _db.AccomodationRequests
                .Where(x => x.Requester.Id == command.RequesterId)
                .ToListAsync(cancellationToken);

            var isOverlap = requests.Any(x => IsOverlap(x.AccomodationStartDateUtc, x.AccomodationEndDateUtc, command.AccomodationStartDateUtc, command.AccomodationEndDateUtc));

            return !isOverlap;
        }

        private bool IsOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 >= end1) throw new ArgumentException("start1 must be less than end1");
            if (start2 >= end2) throw new ArgumentException("start2 must be less than end2");

            return end1 > start2 && start1 < end2;
        }
    }
}
