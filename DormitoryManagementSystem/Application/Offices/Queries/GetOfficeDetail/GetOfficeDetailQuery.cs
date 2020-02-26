using MediatR;

namespace Application.Offices.Queries.GetOfficeDetail
{
    public class GetOfficeDetailQuery : IRequest<OfficeDetail>
    {
        public int Id { get; set; }
    }
}
