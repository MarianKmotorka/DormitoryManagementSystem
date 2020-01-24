using Application.Common.Enums;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading;
using System.Threading.Tasks;

namespace Application.System
{
    public class MigrateAndSeedDbHandler : IRequestHandler<MigrateAndSeedDbCommand>
    {
        private IDormitoryDbContext _db;
        private RoleManager<IdentityRole> _roleManager;

        public MigrateAndSeedDbHandler(IDormitoryDbContext db, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _roleManager = roleManager;
        }

        public async Task<Unit> Handle(MigrateAndSeedDbCommand request, CancellationToken cancellationToken)
        {
            var dbAlreadyExisted = (_db.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            await _db.Database.MigrateAsync(cancellationToken);

            if (dbAlreadyExisted) return Unit.Value;

            await SeedRoles();

            return Unit.Value;
        }

        private async Task SeedRoles()
        {
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.Guest.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.Officer.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.SysAdmin.ToString() });
            await _roleManager.CreateAsync(new IdentityRole { Name = AppRoleNames.Repairer.ToString() });
        }
    }
}
