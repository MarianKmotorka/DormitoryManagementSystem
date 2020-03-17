using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Guests.Commands.DeleteGuest
{
    public class DeleteGuestCommandHandler : IRequestHandler<DeleteGuestCommand>
    {
        private readonly IIdentityService _identityService;
        private readonly IDormitoryDbContext _db;

        public DeleteGuestCommandHandler(IIdentityService identityService, IDormitoryDbContext db)
        {
            _identityService = identityService;
            _db = db;
        }

        public async Task<Unit> Handle(DeleteGuestCommand request, CancellationToken cancellationToken)
        {
            _ = await _db.Guests.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            var result = await _identityService.DeleteUser(request.Id);

            return result.Succeeded ? Unit.Value : throw new BadRequestException(result.Errors);
        }
    }
}
