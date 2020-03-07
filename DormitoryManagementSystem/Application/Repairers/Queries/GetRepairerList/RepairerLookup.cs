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

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Repairer, RepairerLookup>()
                 .ForMember(dest => dest.LastName, cfg =>
                 {
                     cfg.MapFrom(src => src.AppUser.LastName);
                 })
                .ForMember(dest => dest.FirstName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.FirstName);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<RepairerLookup>(x => x.FirstName)
                .CanFilter().CanSort();

            mapper.Property<RepairerLookup>(x => x.LastName)
               .CanFilter().CanSort();
        }
    }
}
