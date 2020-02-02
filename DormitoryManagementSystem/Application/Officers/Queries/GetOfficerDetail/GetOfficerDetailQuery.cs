using MediatR;

namespace Application.Officers.Queries.GetOfficerDetail
{
    public class GetOfficerDetailQuery : IRequest<OfficerDetail>
    {
        public string Id { get; set; }
    }
}
