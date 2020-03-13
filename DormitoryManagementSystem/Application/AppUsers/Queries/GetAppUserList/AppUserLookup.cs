using Application.Common.Mappings;
using Application.Common.Pagination;
using Domain.Entities;
using Sieve.Services;

namespace Application.AppUsers.Queries.GetAppUserList
{
    public class AppUserLookup : IMapFrom<AppUser>, IFilteringSortingProfile
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<AppUserLookup>(x => x.Email)
                .CanFilter().CanSort();
        }
    }
}
