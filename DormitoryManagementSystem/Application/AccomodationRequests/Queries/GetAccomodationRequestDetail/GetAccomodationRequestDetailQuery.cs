using MediatR;

namespace Application.AccomodationRequests.Queries.GetAccomodationRequestDetail
{
    public class GetAccomodationRequestDetailQuery : IRequest<AccomodationRequestDetail>
    {
        public int Id { get; set; }
    }
}
