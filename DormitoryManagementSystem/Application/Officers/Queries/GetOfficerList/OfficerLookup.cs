using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Officers.Queries.GetOfficerList
{
    public class OfficerLookup : IMapFrom<Officer>, IFilteringMapperProfile
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string OfficeNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Officer, OfficerLookup>()
                .ForMember(dest => dest.DisplayName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName + " " + src.AppUser.FirstName);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<OfficerLookup>(x => x.OfficeNumber)
                .CanFilter().CanSort();

            mapper.Property<OfficerLookup>(x => x.DisplayName)
                .CanFilter().CanSort();
        }
    }
}
