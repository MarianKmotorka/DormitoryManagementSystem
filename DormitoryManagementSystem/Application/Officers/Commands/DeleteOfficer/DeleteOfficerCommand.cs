using MediatR;

namespace Application.Officers.Commands.DeleteOfficer
{
    public class DeleteOfficerCommand : IRequest
    {
        public string Id { get; set; }
    }
}
