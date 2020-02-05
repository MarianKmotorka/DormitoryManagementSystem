using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Application.Common.Interfaces
{
    public interface IDormitoryDbContext : IDisposable
    {
        DbSet<Officer> Officers { get; set; }

        DbSet<Guest> Guests { get; set; }

        DbSet<AppUser> Users { get; set; }

        DbSet<RefreshToken> RefreshTokens { get; set; }

        DbSet<AccomodationRequest> AccomodationRequests { get; set; }

        DatabaseFacade Database { get; }

        EntityEntry<TEntity> Attach<TEntity>([NotNull] TEntity entity) where TEntity : class;

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
