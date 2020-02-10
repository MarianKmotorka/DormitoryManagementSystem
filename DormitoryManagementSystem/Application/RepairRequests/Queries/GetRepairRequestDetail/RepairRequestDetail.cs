using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.RepairRequests.Queries.GetRepairRequestDetail
{
    public class RepairRequestDetail : IMapFrom<RepairRequest>
    {
        public int Id { get; set; }

        public string RoomNumber { get; set; }

        public string FixedByDisplayName { get; set; }

        public string ReportedByDisplayName { get; set; }

        public RoomItemTypeDto RoomItemType { get; set; }

        public RepairRequestState State { get; set; }

        public DateTime? WillBeFixedOn { get; set; }

        public DateTime? FixedOn { get; set; }

        public DateTime CreatedOn { get; set; }

        public string ProblemDesciption { get; set; }

        public string RepairerReply { get; set; }

        public class RoomItemTypeDto
        {
            public string InventoryNumber { get; set; }

            public string Name { get; set; }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<RepairRequest, RepairRequestDetail>()
                .ForMember(dest => dest.RoomNumber, cfg =>
                {
                    cfg.MapFrom(src => src.RoomItemType.Room.Number);
                })
                .ForMember(dest => dest.ReportedByDisplayName, cfg =>
                {
                    cfg.MapFrom(src => src.Guest.AppUser.LastName + " " + src.Guest.AppUser.FirstName);
                })
                .ForMember(dest => dest.FixedByDisplayName, cfg =>
                {
                    cfg.MapFrom(src => src.FixedBy.AppUser.LastName + " " + src.FixedBy.AppUser.FirstName);
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
