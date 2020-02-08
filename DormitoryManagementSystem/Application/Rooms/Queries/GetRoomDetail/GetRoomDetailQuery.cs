using MediatR;

namespace Application.Rooms.Queries.GetRoomDetail
{
    public class GetRoomDetailQuery : IRequest<RoomDetail>
    {
        public int? Id { get; set; }

        public string GuestId { get; set; }
    }
}
