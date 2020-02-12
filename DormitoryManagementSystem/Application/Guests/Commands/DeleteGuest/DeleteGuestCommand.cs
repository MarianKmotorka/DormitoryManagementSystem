using MediatR;

namespace Application.Guests.Commands.DeleteGuest
{
    public class DeleteGuestCommand : IRequest
    {
        public string Id { get; set; }
    }
}
