using MediatR;

namespace Application.Repairers.Queries.GetRepairerDetail
{
    public class GetRepairerDetailQuery : IRequest<RepairerDetail>
    {
        public string Id { get; set; }
    }
}
