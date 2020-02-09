using System;
using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Sieve.Services;

namespace Application.RepairRequests.Queries.GetRepairRequestList
{
    public class RepairRequestLookup : IMapFrom<RepairRequest>, IFilteringSortingProfile
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? WillBeFixedOn { get; set; }

        public DateTime? FixedOn { get; set; }

        public RepairRequestState State { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap<RepairRequest, RepairRequestLookup>();

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<RepairRequestLookup>(x => x.CreatedOn)
                .CanFilter().CanSort();

            mapper.Property<RepairRequestLookup>(x => x.WillBeFixedOn)
                .CanFilter().CanSort();

            mapper.Property<RepairRequestLookup>(x => x.FixedOn)
                .CanFilter().CanSort();

            mapper.Property<RepairRequestLookup>(x => x.State)
                .CanFilter().CanSort();
        }
    }
}
