using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Officers.Commands.EditOfficer
{
    public class EditOfficerCommandHandler : IRequestHandler<EditOfficerCommand>
    {
        private readonly IDormitoryDbContext _db;

        public EditOfficerCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(EditOfficerCommand request, CancellationToken cancellationToken)
        {
            var officer = await _db.Officers
                .Include(x => x.AppUser)
                .Include(x => x.AppUser.Address)
                .Include(x => x.Office)
                .SingleOrDefaultAsync(x => x.Id == request.Id);

            var office = await _db.Offices.SingleAsync(x => x.Number == request.OfficeNumber);

            officer.AppUser.FirstName = request.FirstName;
            officer.AppUser.LastName = request.LastName;
            officer.AppUser.PhoneNumber = request.PhoneNumber;
            officer.AppUser.Address.Country = request.Country;
            officer.AppUser.Address.City = request.City;
            officer.AppUser.Address.PostCode = request.PostCode;
            officer.AppUser.Address.Street = request.Street;
            officer.AppUser.Address.HouseNumber = request.HouseNumber;
            officer.Office = office;

            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
