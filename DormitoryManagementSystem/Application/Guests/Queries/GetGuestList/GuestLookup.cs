using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Guests.Queries.GetGuestList
{
    public class GuestLookup : IMapFrom<Guest>, IFilteringMapperProfile
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public string RoomNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Guest, GuestLookup>()
                .ForMember(dest => dest.DisplayName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName + " " + src.AppUser.FirstName);
                })
                .ForMember(dest => dest.RoomNumber, cfg =>
                {
                    cfg.MapFrom(src => src.Room.Number);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<GuestLookup>(x => x.DisplayName)
                .CanFilter().CanSort();

            mapper.Property<GuestLookup>(x => x.RoomNumber)
                .CanSort().CanFilter();
        }
    }
}
