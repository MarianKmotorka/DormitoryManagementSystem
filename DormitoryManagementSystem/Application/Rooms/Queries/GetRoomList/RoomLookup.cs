using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Sieve.Services;

namespace Application.Rooms.Queries.GetRoomList
{
    public class RoomLookup : IMapFrom<Room>, IFilteringSortingProfile
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public int Capacity { get; set; }

        public int FreeBeds { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Room, RoomLookup>()
                .ForMember(dest => dest.RoomNumber, cfg =>
                {
                    cfg.MapFrom(src => src.Number);
                })
                .ForMember(dest => dest.FreeBeds, cfg =>
                {
                    cfg.MapFrom(src => src.Capacity - src.Guests.Count);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<RoomLookup>(x => x.FreeBeds)
                .CanFilter().CanSort();

            mapper.Property<RoomLookup>(x => x.Capacity)
                .CanFilter().CanSort();

            mapper.Property<RoomLookup>(x => x.RoomNumber)
                .CanFilter().CanSort();
        }
    }
}
