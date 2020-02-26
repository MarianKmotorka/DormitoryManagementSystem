using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Offices.Queries.GetOfficeList
{
    public class OfficeLookup : IMapFrom<Office>, IFilteringSortingProfile
    {
        public int Id { get; set; }

        public string OfficeNumber { get; set; }

        public int Capacity { get; set; }

        public int FreeTables { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Office, OfficeLookup>()
                .ForMember(dest => dest.OfficeNumber, cfg =>
                {
                    cfg.MapFrom(src => src.Number);
                })
                .ForMember(dest => dest.FreeTables, cfg =>
                {
                    cfg.MapFrom(src => src.Capacity - src.Officers.Count);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<OfficeLookup>(x => x.FreeTables)
                .CanFilter().CanSort();

            mapper.Property<OfficeLookup>(x => x.Capacity)
                .CanFilter().CanSort();

            mapper.Property<OfficeLookup>(x => x.OfficeNumber)
                .CanFilter().CanSort();
        }
    }
}
