using System;
using Application.Common.Mappings;
using Application.Common.Pagination;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Sieve.Services;

namespace Application.AccomodationRequests.Queries.GetAccomodationRequestList
{
    public class AccomodationRequestLookup : IMapFrom<AccomodationRequest>, IFilteringMapperProfile
    {
        public int Id { get; set; }

        public int RequesterDistanceFromHome { get; set; }

        public DateTime AccomodationStartDateUtc { get; set; }

        public DateTime AccomodationEndDateUtc { get; set; }

        public AccomodationRequestState RequestState { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccomodationRequest, AccomodationRequestLookup>()
                .ForMember(dest => dest.RequesterDistanceFromHome, cfg =>
                {
                    cfg.MapFrom(src => src.Requester.DistanceFromHome);
                })
                .ForMember(dest => dest.RequestState, cfg =>
                {
                    cfg.MapFrom(src => src.State);
                });
        }

        public void MapProperties(SievePropertyMapper mapper)
        {
            mapper.Property<AccomodationRequestLookup>(x => x.RequestState)
                .CanFilter();

            mapper.Property<AccomodationRequestLookup>(x => x.AccomodationStartDateUtc)
                .CanFilter().CanSort();

            mapper.Property<AccomodationRequestLookup>(x => x.AccomodationEndDateUtc)
                .CanFilter().CanSort();


            mapper.Property<AccomodationRequestLookup>(x => x.RequesterDistanceFromHome)
                .CanFilter().CanSort();
        }
    }
}
