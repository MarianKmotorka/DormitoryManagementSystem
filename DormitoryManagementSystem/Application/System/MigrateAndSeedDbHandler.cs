using System.Threading;
using System.Threading.Tasks;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace Application.System
{
    public class MigrateAndSeedDbHandler : IRequestHandler<MigrateAndSeedDbCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public MigrateAndSeedDbHandler(IDormitoryDbContext db, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Unit> Handle(MigrateAndSeedDbCommand request, CancellationToken cancellationToken)
        {
            var dbAlreadyExisted = (_db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            await _db.Database.MigrateAsync(cancellationToken);

            if (dbAlreadyExisted) return Unit.Value;

            await SeedRoles();
            await SeedAdmin();
            await SeedGuests();
            await SeedOfficers();

            return Unit.Value;
        }

        private async Task SeedRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.Guest.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.Officer.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.SysAdmin.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.Repairer.ToString() });
        }

        private async Task SeedAdmin()
        {
            var admin = new AppUser
            {
                Id = "d51f39ec-8882-4917-899d-cad4a24f113d",
                Email = "admin@admin.com",
                UserName = "admin",
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(admin, "admin123");

            await _userManager.AddToRoleAsync(admin, AppRoleNames.SysAdmin.ToString());
        }

        private async Task SeedGuests()
        {
            var john = new AppUser
            {
                Email = "john@guest.com",
                UserName = "john@guest.com",
                EmailConfirmed = true
            };

            var bob = new AppUser
            {
                Email = "bob@guest.com",
                UserName = "bob@guest.com",
                EmailConfirmed = true
            };

            var jerry = new AppUser
            {
                Email = "jerry@guest.com",
                UserName = "jerry@guest.com",
                EmailConfirmed = true
            };

            var appUsers = new[] { john, bob, jerry };

            foreach (var appUser in appUsers)
            {
                await _userManager.CreateAsync(appUser, "string");
                await _userManager.AddToRoleAsync(appUser, AppRoleNames.Guest.ToString());
            }

            var guests = new[]
            {
                new Guest
                {
                    AppUser = _db.Attach(john).Entity,
                    DistanceFromHome=140,
                    IdCardNumber="ER7895"
                },
                new Guest
                {
                    AppUser=_db.Attach(bob).Entity,
                    DistanceFromHome=50,
                    IdCardNumber="es5646"
                },
                new Guest
                {
                    AppUser = _db.Attach(jerry).Entity,
                    DistanceFromHome = 40,
                    IdCardNumber="gr243511"
                }
            };

            foreach (var guest in guests)
                _db.Guests.Add(guest);

            await _db.SaveChangesAsync(CancellationToken.None);
        }

        private async Task SeedOfficers()
        {
            var john = new AppUser
            {
                Email = "john@officer.com",
                UserName = "john@officer.com",
                EmailConfirmed = true
            };

            var bob = new AppUser
            {
                Email = "bob@officer.com",
                UserName = "bob@officer.com",
                EmailConfirmed = true
            };

            var jerry = new AppUser
            {
                Email = "jerry@officer.com",
                UserName = "jerry@officer.com",
                EmailConfirmed = true
            };

            var appUsers = new[] { john, bob, jerry };

            foreach (var appUser in appUsers)
            {
                await _userManager.CreateAsync(appUser, "string");
                await _userManager.AddToRoleAsync(appUser, AppRoleNames.Officer.ToString());
            }

            var officers = new[]
            {
                new Officer
                {
                    AppUser = _db.Attach(john).Entity,
                    IdCardNumber="ER78455",
                    OfficeNumber="O456"
                },
                new Officer
                {
                    AppUser=_db.Attach(bob).Entity,
                    IdCardNumber="re5646",
                    OfficeNumber="O789"
                },
                new Officer
                {
                    AppUser = _db.Attach(jerry).Entity,
                    IdCardNumber="YY243511",
                    OfficeNumber="O357"
                }
            };

            foreach (var officer in officers)
                _db.Officers.Add(officer);

            await _db.SaveChangesAsync(CancellationToken.None);
        }
    }
}
