using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Repairers.Commands.EditRepairer
{
    public class EditRepairerCommandHandler : IRequestHandler<EditRepairerCommand>
    {
        private readonly IDormitoryDbContext _db;

        public EditRepairerCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(EditRepairerCommand request, CancellationToken cancellationToken)
        {
            var repairers = await _db.Repairers
                .Include(x => x.AppUser)
                .Include(x => x.AppUser.Address)
                .SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            repairers.AppUser.FirstName = request.FirstName;
            repairers.AppUser.LastName = request.LastName;
            repairers.AppUser.PhoneNumber = request.PhoneNumber;
            repairers.AppUser.Address.Country = request.Country;
            repairers.AppUser.Address.City = request.City;
            repairers.AppUser.Address.PostCode = request.PostCode;
            repairers.AppUser.Address.Street = request.Street;
            repairers.AppUser.Address.HouseNumber = request.HouseNumber;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
