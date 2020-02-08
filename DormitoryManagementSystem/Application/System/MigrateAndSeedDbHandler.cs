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
            await SeedRooms();

            return Unit.Value;
        }

        private async Task SeedRooms()
        {
            for (int i = 1; i < 200; i++)
            {
                var room = new Room
                {
                    Capacity = i % 2 == 0 ? 2 : 3,
                    Number = $"SI{i:D3}"
                };

                _db.Rooms.Add(room);
            }

            await _db.SaveChangesAsync(CancellationToken.None);
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
                Id = "ee552a4a-a6a3-4de1-b981-7ece2d0a8350",
                Email = "john@guest.com",
                UserName = "john@guest.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Cena"
            };

            var bob = new AppUser
            {
                Id = "4f61d7f5-24f1-4d1d-8c79-80d442807f95",
                Email = "bob@guest.com",
                UserName = "bob@guest.com",
                EmailConfirmed = true,
                FirstName = "Bob",
                LastName = "Ross"
            };

            var jerry = new AppUser
            {
                Id = "b42f5d99-b313-45d0-88f3-38427f98c100",
                Email = "jerry@guest.com",
                UserName = "jerry@guest.com",
                EmailConfirmed = true,
                FirstName = "Jerry",
                LastName = "Rig"
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
                Id = "eb523e86-fec9-4745-885a-4bd1f4508bb5",
                Email = "john@officer.com",
                UserName = "john@officer.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Travolta"
            };

            var bob = new AppUser
            {
                Id = "6d22ff48-1098-4ef2-bd5e-da4dad83afab",
                Email = "bob@officer.com",
                UserName = "bob@officer.com",
                EmailConfirmed = true,
                FirstName = "Bob",
                LastName = "Johnes"
            };

            var jerry = new AppUser
            {
                Id = "918ddc96-fa66-45c0-92c7-60c82298cb33",
                Email = "jerry@officer.com",
                UserName = "jerry@officer.com",
                EmailConfirmed = true,
                FirstName = "Jerry",
                LastName = "Everything"
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
