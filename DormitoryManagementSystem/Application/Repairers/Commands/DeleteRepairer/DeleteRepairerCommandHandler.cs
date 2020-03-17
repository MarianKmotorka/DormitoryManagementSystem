using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Repairers.Commands.DeleteRepairer
{
    public class DeleteRepairerCommandHandler : IRequestHandler<DeleteRepairerCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identityService;

        public DeleteRepairerCommandHandler(IDormitoryDbContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteRepairerCommand request, CancellationToken cancellationToken)
        {
            _ = await _db.Repairers.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            var result = await _identityService.DeleteUser(request.Id);

            return result.Succeeded ? Unit.Value : throw new BadRequestException(result.Errors);
        }
    }
}
