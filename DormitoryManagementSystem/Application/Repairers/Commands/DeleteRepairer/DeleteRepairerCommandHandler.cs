using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Repairers.Commands.DeleteRepairer
{
    public class DeleteRepairerCommandHandler : IRequestHandler<DeleteRepairerCommand>
    {
        private readonly IDormitoryDbContext _db;

        public DeleteRepairerCommandHandler(IDormitoryDbContext db)
        {
            _db = db;
        }

        public async Task<Unit> Handle(DeleteRepairerCommand request, CancellationToken cancellationToken)
        {
            var repairer = await _db.Repairers.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            _db.Repairers.Remove(repairer);
            await _db.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
