using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IDormitoryDbContext : IDisposable
    {
        DbSet<Officer> Officers { get; set; }
        DbSet<Guest> Guests { get; set; }
        DbSet<AppUser> Users { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }

        DatabaseFacade Database { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
