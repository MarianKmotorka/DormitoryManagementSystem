using MediatR;

namespace Application.Guests.Queries.GetGuestDetail
{
    public class GetGuestDetailQuery : IRequest<GuestDetail>
    {
        public string Id { get; set; }
    }
}
