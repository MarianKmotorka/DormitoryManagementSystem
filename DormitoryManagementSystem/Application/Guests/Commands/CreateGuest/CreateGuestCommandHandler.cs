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

namespace Application.Guests.Commands.CreateGuest
{
    public class CreateGuestCommandHandler : IRequestHandler<CreateGuestCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identitySerivice;
        private readonly IMediator _mediator;

        public CreateGuestCommandHandler(IDormitoryDbContext db, IIdentityService identitySerivice, IMediator mediator)
        {
            _db = db;
            _identitySerivice = identitySerivice;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {
            var (result, userId) = await _identitySerivice.RegisterUserAsync(request.FirstName, request.LastName, request.Email, request.Password, AppRoleNames.Guest);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            var appUser = await _db.Users.SingleAsync(x => x.Id == userId, cancellationToken);

            var guestAddress = new Address
            {
                City = request.City,
                Country = request.Country,
                HouseNumber = request.HouseNumber,
                PostCode = request.PostCode,
                Street = request.Street
            };

            appUser.PhoneNumber = request.PhoneNumber;
            appUser.Address = guestAddress;

            await _db.SaveChangesAsync(cancellationToken);

            var guest = new Guest
            {
                AppUser = appUser,
                IdCardNumber = request.IdCardNumber,
                DistanceFromHome = request.DistanceFromHome,
            };

            _db.Guests.Add(guest);

            await _db.SaveChangesAsync(cancellationToken);
            await _mediator.Send(new SendConfirmationEmailCommand { Email = request.Email });

            return Unit.Value;
        }
    }
}
