using MediatR;

namespace Application.Repairers.Commands.DeleteRepairer
{
    public class DeleteRepairerCommand : IRequest
    {
        public string Id { get; set; }
    }
}
