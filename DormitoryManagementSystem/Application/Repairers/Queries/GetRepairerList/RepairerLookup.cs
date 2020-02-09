using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Repairers.Queries.GetRepairerList
{
    public class RepairerLookup : IMapFrom<Repairer>, IFilteringSortingProfile
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Repairer, RepairerLookup>()
                .ForMember(dest => dest.DisplayName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName + " " + src.AppUser.FirstName);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<RepairerLookup>(x => x.DisplayName)
                .CanFilter().CanSort();
        }
    }
}
