using Application.AppUsers.Commands.SendChangePasswordEmail;
using Application.AppUsers.Commands.SendConfirmationEmail;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Officers.Commands.CreateOfficer
{
    public class CreateOfficerCommandHanlder : IRequestHandler<CreateOfficerCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identitySerivice;
        private readonly IMediator _mediator;

        public CreateOfficerCommandHanlder(IDormitoryDbContext db, IIdentityService identitySerivice, IMediator mediator)
        {
            _db = db;
            _identitySerivice = identitySerivice;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateOfficerCommand request, CancellationToken cancellationToken)
        {
            var password = Guid.NewGuid().ToString();

            var (result, userId) = await _identitySerivice.RegisterUserAsync(request.Email, password, AppRoleNames.Officer);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.FirstOrDefault());

            var appUser = await _db.Users.SingleAsync(x => x.Id == userId, cancellationToken);

            var officerAddress = new Address
            {
                City = request.City,
                Country = request.Country,
                HouseNumber = request.HouseNumber,
                PostCode = request.PostCode,
                Street = request.Street
            };

            appUser.FirstName = request.FirstName;
            appUser.LastName = request.LastName;
            appUser.PhoneNumber = request.PhoneNumber;
            appUser.Address = officerAddress;

            await _db.SaveChangesAsync(cancellationToken);

            var officer = new Officer
            {
                AppUser = appUser,
                IdCardNumber = request.IdCardNumber,
                OfficeNumber = request.OfficeNumber
            };

            _db.Officers.Add(officer);

            await _db.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new SendConfirmationEmailCommand { Email = request.Email });
            await _mediator.Send(new SendChangePasswordEmailCommand
            {
                Email = request.Email,
                AdditionalMessage = $"Now you can log in using email: { request.Email} and password: {password}. Please change your password ASAP."
            });

            return Unit.Value;
        }
    }
}
