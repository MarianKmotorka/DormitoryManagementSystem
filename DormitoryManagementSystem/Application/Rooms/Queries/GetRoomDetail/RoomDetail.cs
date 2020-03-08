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

        public IEnumerable<RoomItemTypeDto> Items { get; set; }

        public class GuestDto
        {
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        public class RoomItemTypeDto
        {
            public int Id { get; set; }

            public string InventoryNumber { get; set; }

            public string Name { get; set; }

            public int Quantity { get; set; }
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
                .ForMember(dest => dest.FirstName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.FirstName);
                })
                .ForMember(dest => dest.LastName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName);
                });

            profile.CreateMap<RoomItemType, RoomItemTypeDto>()
                .ForMember(dest => dest.InventoryNumber, cfg =>
                {
                    cfg.MapFrom(src => src.InventoryItemType.InventoryNumber);
                })
                .ForMember(dest => dest.Name, cfg =>
                {
                    cfg.MapFrom(src => src.InventoryItemType.Name);
                });
        }
    }
}
