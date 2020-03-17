using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Officers.Commands.DeleteOfficer
{
    public class DeleteOfficerCommandHandler : IRequestHandler<DeleteOfficerCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identityService;

        public DeleteOfficerCommandHandler(IDormitoryDbContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteOfficerCommand request, CancellationToken cancellationToken)
        {
            _ = await _db.Officers.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            var result = await _identityService.DeleteUser(request.Id);

            return result.Succeeded ? Unit.Value : throw new BadRequestException(result.Errors);
        }
    }
}
