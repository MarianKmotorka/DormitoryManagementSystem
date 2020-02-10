using MediatR;

namespace Application.RepairRequests.Queries.GetRepairRequestDetail
{
    public class GetRepairRequestDetailQuery : IRequest<RepairRequestDetail>
    {
        public int Id { get; set; }

        public string GuestId { get; set; }
    }
}
