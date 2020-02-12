using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Officers.Commands.DeleteOfficer
{
    public class DeleteOfficerCommandHandler : IRequestHandler<DeleteOfficerCommand>
    {
        private readonly IDormitoryDbContext _db;

        public DeleteOfficerCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteOfficerCommand request, CancellationToken cancellationToken)
        {
            var officer = await _db.Officers.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            _db.Officers.Remove(officer);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
