using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.AppUsers.Commands.ChangePasswordByAdmin
{
    public class ChangePasswordByAdminCommandHandler : IRequestHandler<ChangePasswordByAdminCommand>
    {
        private readonly IDormitoryDbContext _db;
        private readonly IIdentityService _identityService;

        public ChangePasswordByAdminCommandHandler(IDormitoryDbContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(ChangePasswordByAdminCommand request, CancellationToken cancellationToken)
        {
            _ = await _db.Users.SingleOrNotFoundAsync(x => x.Id == request.Id, cancellationToken);

            var result = await _identityService.ChangePasswordAsync(request.Id, request.NewPassword);

            return result.Succeeded ? Unit.Value : throw new BadRequestException(result.Errors);
        }
    }
}
