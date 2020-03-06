using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Guests.Commands.EditGuest
{
    public class EditGuestCommandHandler : IRequestHandler<EditGuestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public EditGuestCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(EditGuestCommand request, CancellationToken cancellationToken)
        {
            var guest = await _db.Guests
                .Include(x => x.AppUser)
                .Include(x => x.AppUser.Address)
                .Include(x => x.Room)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            var room = await _db.Rooms.SingleOrDefaultAsync(x => x.Number == request.RoomNumber, cancellationToken);

            guest.AppUser.FirstName = request.FirstName;
            guest.AppUser.LastName = request.LastName;
            guest.AppUser.PhoneNumber = request.PhoneNumber;
            guest.DistanceFromHome = request.DistanceFromHome;
            guest.Room = room;
            guest.AppUser.Address.Country = request.Country;
            guest.AppUser.Address.City = request.City;
            guest.AppUser.Address.Street = request.Street;
            guest.AppUser.Address.PostCode = request.PostCode;
            guest.AppUser.Address.Street = request.Street;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
