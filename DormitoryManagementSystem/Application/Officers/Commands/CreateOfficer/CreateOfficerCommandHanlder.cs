using System;
using System.Threading;
using System.Threading.Tasks;
using Application.AppUsers.Commands.SendConfirmationEmail;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Officers.Commands.CreateOfficer
{
    public class CreateOfficerCommandHanlder : IRequestHandler<CreateOfficerCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;

        public CreateOfficerCommandHanlder(IDormitoryDbContext db, IIdentityService identityService, IMediator mediator,
            IEmailService emailService)
        {
            _db = db;
            _identityService = identityService;
            _mediator = mediator;
            _emailService = emailService;
        }

        public async Task<Unit> Handle(CreateOfficerCommand request, CancellationToken cancellationToken)
        {
            var password = Guid.NewGuid().ToString().Substring(0, 6) + "x+2";

            var (result, userId) = await _identityService.RegisterUserAsync(request.FirstName, request.LastName, request.Email, password, AppRoleNames.Officer);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            var appUser = await _db.Users.SingleAsync(x => x.Id == userId, cancellationToken);

            var officerAddress = new Address
            {
                City = request.City,
                Country = request.Country,
                HouseNumber = request.HouseNumber,
                PostCode = request.PostCode,
                Street = request.Street
            };

            appUser.PhoneNumber = request.PhoneNumber;
            appUser.Address = officerAddress;

            await _db.SaveChangesAsync(cancellationToken);

            var office = await _db.Offices.SingleAsync(x => x.Number == request.OfficeNumber);

            var officer = new Officer
            {
                AppUser = appUser,
                IdCardNumber = request.IdCardNumber,
                Office = office
            };

            _db.Officers.Add(officer);

            await _db.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new SendConfirmationEmailCommand { Email = request.Email });

            await _emailService.SendAsync(
                $"Now you can log in using email: {request.Email} and password: {password}  . Please change your password ASAP.",
                request.Email,
                "Credentials",
                isMessageHtml: false
                );

            return Unit.Value;
        }
    }
}
