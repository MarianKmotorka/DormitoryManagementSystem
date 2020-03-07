using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Officers.Queries.GetOfficerList
{
    public class OfficerLookup : IMapFrom<Officer>, IFilteringSortingProfile
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string OfficeNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Officer, OfficerLookup>()
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
            mapper.Property<OfficerLookup>(x => x.OfficeNumber)
                .CanFilter().CanSort();

            mapper.Property<OfficerLookup>(x => x.FirstName)
                .CanFilter().CanSort();

            mapper.Property<OfficerLookup>(x => x.LastName)
                .CanFilter().CanSort();
        }
    }
}
