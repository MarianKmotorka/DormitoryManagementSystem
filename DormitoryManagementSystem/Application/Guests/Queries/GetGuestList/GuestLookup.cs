using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Guests.Queries.GetGuestList
{
    public class GuestLookup : IMapFrom<Guest>, IFilteringSortingProfile
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string RoomNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Guest, GuestLookup>()
                .ForMember(dest => dest.FirstName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.FirstName);
                })
                .ForMember(dest => dest.LastName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName);
                })
                .ForMember(dest => dest.RoomNumber, cfg =>
                {
                    cfg.MapFrom(src => src.Room.Number);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<GuestLookup>(x => x.FirstName)
                .CanFilter().CanSort();

            mapper.Property<GuestLookup>(x => x.LastName)
                .CanFilter().CanSort();

            mapper.Property<GuestLookup>(x => x.RoomNumber)
                .CanSort().CanFilter();
        }
    }
}
