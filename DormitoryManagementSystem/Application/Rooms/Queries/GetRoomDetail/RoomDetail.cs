using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Rooms.Queries.GetRoomDetail
{
    public class RoomDetail : IMapFrom<Room>
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public int Capacity { get; set; }

        public int FreeBeds { get; set; }

        public IEnumerable<GuestDto> Guests { get; set; }

        public class GuestDto
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Room, RoomDetail>()
                .ForMember(dest => dest.FreeBeds, cfg =>
                {
                    cfg.MapFrom(src => src.Capacity - src.Guests.Count());
                })
                .ForMember(dest => dest.RoomNumber, cfg =>
                {
                    cfg.MapFrom(src => src.Number);
                });

            profile.CreateMap<Guest, GuestDto>()
            .ForMember(dest => dest.DisplayName, cfg =>
            {
                cfg.MapFrom(src => src.AppUser.LastName + " " + src.AppUser.FirstName);
            });
        }
    }
}
