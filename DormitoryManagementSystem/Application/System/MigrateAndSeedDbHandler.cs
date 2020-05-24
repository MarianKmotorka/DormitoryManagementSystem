using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Application.System
{
    public class MigrateAndSeedDbHandler : IRequestHandler<MigrateAndSeedDbCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<MigrateAndSeedDbHandler> _logger;
        private readonly int _numberOfRooms = 200;
        private readonly Random _random;

        public MigrateAndSeedDbHandler(IDormitoryDbContext db, RoleManager<IdentityRole> roleManager,
            UserManager<AppUser> userManager, ILogger<MigrateAndSeedDbHandler> logger)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
            _random = new Random();
        }

        public async Task<Unit> Handle(MigrateAndSeedDbCommand request, CancellationToken cancellationToken)
        {
            var dbAlreadyExisted = (_db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            _logger.LogInformation("Looking for database. If database does not exist, it will be created.\nThis should not take more than 1 minute.");
            await _db.Database.MigrateAsync(cancellationToken);

            if (dbAlreadyExisted) return Unit.Value;

            _logger.LogInformation("Seeding database with some test data ...");

            await SeedRoles();
            await SeedAdmin();
            await SeedGuests();
            await SeedRepairers();
            await SeedOffices();
            await SeedOfficers();
            await SeedRooms();
            await SeedInventoryItemTypes();
            await SeedRoomItemTypes();
            await SeedAccomodationRequests();

            _logger.LogInformation("Database is ready");

            return Unit.Value;
        }

        private async Task SeedOffices()
        {
            Enumerable.Range(1, 10)
                .ToList()
                .ForEach(x => _db.Offices.Add(new Office
                {
                    Number = $"K{x:D3}",
                    Capacity = 4
                }));

            await _db.SaveChangesAsync(default);
        }

        private async Task SeedRoomItemTypes()
        {
            var inventoryItemTypes = await _db.InventoryItemTypes.ToListAsync();
            var rooms = await _db.Rooms.ToListAsync();

            var chair = inventoryItemTypes.Single(x => x.Name == "Chair");
            var table = inventoryItemTypes.Single(x => x.Name == "Table");
            var lamp = inventoryItemTypes.Single(x => x.Name == "Lamp");
            var ceilingLight = inventoryItemTypes.Single(x => x.Name == "Ceiling light");
            var bed = inventoryItemTypes.Single(x => x.Name == "Bed");
            var cupboard = inventoryItemTypes.Single(x => x.Name == "Cupboard");
            var wardrobe = inventoryItemTypes.Single(x => x.Name == "Wardrobe");

            foreach (var room in rooms)
                room.Items = new[]
                {
                    new RoomItemType
                    {
                        InventoryItemType = chair,
                        Quantity = room.Capacity,
                    },
                    new RoomItemType
                    {
                        InventoryItemType = table,
                        Quantity = room.Capacity,
                    },
                    new RoomItemType
                    {
                        InventoryItemType = lamp,
                        Quantity = room.Capacity,
                    },
                    new RoomItemType
                    {
                        InventoryItemType = ceilingLight,
                        Quantity = 1,
                    },
                    new RoomItemType
                    {
                        InventoryItemType = bed,
                        Quantity = room.Capacity,
                    },
                    new RoomItemType
                    {
                        InventoryItemType = cupboard,
                        Quantity = room.Capacity,
                    },
                    new RoomItemType
                    {
                        InventoryItemType = wardrobe,
                        Quantity = room.Capacity,
                    }
                };

            await _db.SaveChangesAsync(CancellationToken.None);
        }

        private async Task SeedInventoryItemTypes()
        {
            var chairType = new InventoryItemType
            {
                Name = "Chair",
                InventoryNumber = "IIT001",
                PricePerPiece = 30,
                TotalQuantity = _numberOfRooms * 3
            };

            var tableType = new InventoryItemType
            {
                Name = "Table",
                InventoryNumber = "IIT002",
                PricePerPiece = 100,
                TotalQuantity = _numberOfRooms * 3
            };

            var lampType = new InventoryItemType
            {
                Name = "Lamp",
                InventoryNumber = "IIT003",
                PricePerPiece = 20,
                TotalQuantity = _numberOfRooms * 3
            };

            var ceilingLightType = new InventoryItemType
            {
                Name = "Ceiling light",
                InventoryNumber = "IIT004",
                PricePerPiece = 40,
                TotalQuantity = _numberOfRooms + 20
            };

            var bedType = new InventoryItemType
            {
                Name = "Bed",
                InventoryNumber = "IIT005",
                PricePerPiece = 200,
                TotalQuantity = _numberOfRooms * 3
            };

            var cupboardType = new InventoryItemType
            {
                Name = "Cupboard",
                InventoryNumber = "IIT006",
                PricePerPiece = 20,
                TotalQuantity = _numberOfRooms * 3
            };

            var wardrobeType = new InventoryItemType
            {
                Name = "Wardrobe",
                InventoryNumber = "IIT007",
                PricePerPiece = 200,
                TotalQuantity = _numberOfRooms * 3
            };

            _db.AddRange(chairType, tableType, lampType, ceilingLightType, bedType, cupboardType, wardrobeType);
            await _db.SaveChangesAsync(CancellationToken.None);
        }

        private async Task SeedRooms()
        {
            for (int i = 1; i <= _numberOfRooms; i++)
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
                EmailConfirmed = true,
                FirstName = "Admin",
                LastName = "Admin"
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
                LastName = "Cena",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Washington",
                    Country = "USA",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var bob = new AppUser
            {
                Id = "4f61d7f5-24f1-4d1d-8c79-80d442807f95",
                Email = "bob@guest.com",
                UserName = "bob@guest.com",
                EmailConfirmed = true,
                FirstName = "Bob",
                LastName = "Ross",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Topolcany",
                    Country = "Slovensko",
                    HouseNumber = "95/7",
                    PostCode = "950 19",
                    Street = "Bernolakova"
                }
            };

            var jerry = new AppUser
            {
                Id = "b42f5d99-b313-45d0-88f3-38427f98c100",
                Email = "jerry@guest.com",
                UserName = "jerry@guest.com",
                EmailConfirmed = true,
                FirstName = "Jerry",
                LastName = "Rig",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Wien",
                    Country = "Austria",
                    HouseNumber = "44445",
                    PostCode = "J45",
                    Street = "Blume Strasse"
                }
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
                LastName = "Travolta",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Bratislava",
                    Country = "Slovakia",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var bob = new AppUser
            {
                Id = "6d22ff48-1098-4ef2-bd5e-da4dad83afab",
                Email = "bob@officer.com",
                UserName = "bob@officer.com",
                EmailConfirmed = true,
                FirstName = "Bob",
                LastName = "Johnes",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Gdansk",
                    Country = "Poland",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var jerry = new AppUser
            {
                Id = "918ddc96-fa66-45c0-92c7-60c82298cb33",
                Email = "jerry@officer.com",
                UserName = "jerry@officer.com",
                EmailConfirmed = true,
                FirstName = "Jerry",
                LastName = "Everything",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Poprad",
                    Country = "SLovakia",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var appUsers = new[] { john, bob, jerry };

            foreach (var appUser in appUsers)
            {
                await _userManager.CreateAsync(appUser, "string");
                await _userManager.AddToRoleAsync(appUser, AppRoleNames.Officer.ToString());
            }

            var offices = await _db.Offices.Take(3).ToListAsync();

            var officers = new[]
            {
                new Officer
                {
                    AppUser = _db.Attach(john).Entity,
                    IdCardNumber="ER78455",
                    Office = offices[0]
                },
                new Officer
                {
                    AppUser=_db.Attach(bob).Entity,
                    IdCardNumber="re5646",
                    Office = offices[1]
                },
                new Officer
                {
                    AppUser = _db.Attach(jerry).Entity,
                    IdCardNumber="YY243511",
                    Office = offices[2]
                }
            };

            foreach (var officer in officers)
                _db.Officers.Add(officer);

            await _db.SaveChangesAsync(CancellationToken.None);
        }

        private async Task SeedRepairers()
        {
            var john = new AppUser
            {
                Id = "tt523e86-fec9-4745-885a-4bd1f4508bb5",
                Email = "john@repairer.com",
                UserName = "john@repairer.com",
                EmailConfirmed = true,
                FirstName = "John",
                LastName = "Bostic",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Bratislava",
                    Country = "Slovakia",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var bob = new AppUser
            {
                Id = "6322ff48-1098-4ef2-bd5e-da4dad83afab",
                Email = "bob@repairer.com",
                UserName = "bob@repairer.com",
                EmailConfirmed = true,
                FirstName = "Bob",
                LastName = "Herman",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Gdansk",
                    Country = "Poland",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var jerry = new AppUser
            {
                Id = "ww8ddc96-fa66-45c0-92c7-60c82298cb33",
                Email = "jerry@repairer.com",
                UserName = "jerry@repairer.com",
                EmailConfirmed = true,
                FirstName = "Jerry",
                LastName = "Poolhouse",
                PhoneNumber = GetRandomPhoneNumber(),
                Address = new Address
                {
                    City = "Poprad",
                    Country = "SLovakia",
                    HouseNumber = "X55/9",
                    PostCode = "088 99",
                    Street = "Main road"
                }
            };

            var appUsers = new[] { john, bob, jerry };

            foreach (var appUser in appUsers)
            {
                await _userManager.CreateAsync(appUser, "string");
                await _userManager.AddToRoleAsync(appUser, AppRoleNames.Repairer.ToString());
            }

            var repairers = new[]
            {
                new Repairer
                {
                    AppUser = _db.Attach(john).Entity,
                },
                new Repairer
                {
                    AppUser=_db.Attach(bob).Entity,
                },
                new Repairer
                {
                    AppUser = _db.Attach(jerry).Entity,
                }
            };

            foreach (var repairer in repairers)
                _db.Repairers.Add(repairer);

            await _db.SaveChangesAsync(CancellationToken.None);
        }

        private async Task SeedAccomodationRequests()
        {
            var guests = await _db.Guests.ToListAsync();

            foreach (var guest in guests)
            {
                var accRequest = new AccomodationRequest
                {
                    Requester = guest,
                    AccomodationStartDateUtc = DateTime.Now.AddDays(_random.Next(10, 100)),
                    AccomodationEndDateUtc = DateTime.Now.AddYears(1),
                    RequesterMessage = $"Prosím ubytujte ma na izbu SI{_random.Next(10, _numberOfRooms)}",
                    RequestPlacedUtc = DateTime.Now.AddDays(_random.Next(-30, -1)),
                    State = AccomodationRequestState.Active
                };

                _db.AccomodationRequests.Add(accRequest);
                await _db.SaveChangesAsync(default);
            }
        }

        private string GetRandomPhoneNumber()
        {
            var numberArray = Enumerable.Range(1, 10)
                .Select(_ => _random.Next(0, 10))
                .ToArray();

            return string.Join("", numberArray);
        }
    }
}
