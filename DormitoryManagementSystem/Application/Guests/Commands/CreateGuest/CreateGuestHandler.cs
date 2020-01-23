﻿using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Guests.Commands.CreateGuest
{
    public class CreateGuestHandler : IRequestHandler<CreateGuestCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identitySerivice;

        public CreateGuestHandler(IDormitoryDbContext db, IIdentityService identitySerivice)
        {
            _db = db;
            _identitySerivice = identitySerivice;
        }

        public async Task<Unit> Handle(CreateGuestCommand request, CancellationToken cancellationToken)
        {
            var (result, userId) = await _identitySerivice.RegisterUserAsync(request.Email, request.Password, AppRoleNames.Guest);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors.FirstOrDefault());

            var appUser = await _db.Users.SingleAsync(x => x.Id == userId, cancellationToken);

            var guestAddress = new Address
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

            return Unit.Value;
        }
    }
}
