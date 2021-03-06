﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Application.AppUsers.Commands.SendConfirmationEmail;
using Application.Common.Enums;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace Application.Repairers.Commands.CreateRepairer
{
    public class CreateRepairerCommandHanlder : IRequestHandler<CreateRepairerCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _enviroment;

        public CreateRepairerCommandHanlder(IDormitoryDbContext db, IIdentityService identityService, IMediator mediator,
            IEmailService emailService, IHostingEnvironment enviroment)
        {
            _db = db;
            _identityService = identityService;
            _mediator = mediator;
            _emailService = emailService;
            _enviroment = enviroment;
        }

        public async Task<Unit> Handle(CreateRepairerCommand request, CancellationToken cancellationToken)
        {
            var password = _enviroment.IsDevelopment() ? "string" : Guid.NewGuid().ToString().Substring(0, 6) + "x2*";

            var (result, userId) = await _identityService.RegisterUserAsync(request.FirstName, request.LastName, request.Email, password, AppRoleNames.Repairer);

            if (!result.Succeeded)
                throw new BadRequestException(result.Errors);

            var appUser = await _db.Users.SingleAsync(x => x.Id == userId, cancellationToken);

            var repairerAddress = new Address
            {
                City = request.City,
                Country = request.Country,
                HouseNumber = request.HouseNumber,
                PostCode = request.PostCode,
                Street = request.Street
            };

            appUser.PhoneNumber = request.PhoneNumber;
            appUser.Address = repairerAddress;

            await _db.SaveChangesAsync(cancellationToken);

            var repairer = new Repairer
            {
                AppUser = appUser
            };

            _db.Repairers.Add(repairer);

            await _db.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new SendConfirmationEmailCommand { Email = request.Email });

            await _emailService.SendAsync(
                $"Now you can log in using email: {request.Email} and password: {password}   . Please change your password ASAP.",
                request.Email,
                "Credentials",
                isMessageHtml: false
                );

            return Unit.Value;
        }
    }
}
