using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Guests.Commands.DeleteGuest
{
    public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand>
    {
        private readonly IDormitoryDbContext _db;

        public DeleteGuestCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
        {
            var guest = await _db.Guests.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            _db.Guests.Remove(guest);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
