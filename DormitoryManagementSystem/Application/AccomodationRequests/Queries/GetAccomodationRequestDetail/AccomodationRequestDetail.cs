using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.AccomodationRequests.Queries.GetAccomodationRequestDetail
{
    public class AccomodationRequestDetail : IMapFrom<AccomodationRequest>
    {
        public int Id { get; set; }

        public string RequesterId { get; set; }

        public string RequesterFirstName { get; set; }

        public string RequesterLastName { get; set; }

        public string RequesterEmail { get; set; }

        public string RequesterMessage { get; set; }

        public int RequesterDistanceFromHome { get; set; }

        public DateTime AccomodationStartDateUtc { get; set; }

        public DateTime AccomodationEndDateUtc { get; set; }

        public AccomodationRequestState RequestState { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AccomodationRequest, AccomodationRequestDetail>()
                .ForMember(dest => dest.RequesterDistanceFromHome, cfg =>
                {
                    cfg.MapFrom(src => src.Requester.DistanceFromHome);
                })
                .ForMember(dest => dest.RequestState, cfg =>
                {
                    cfg.MapFrom(src => src.State);
                })
                .ForMember(dest => dest.RequesterFirstName, cfg =>
                {
                    cfg.MapFrom(src => src.Requester.AppUser.FirstName);
                })
                .ForMember(dest => dest.RequesterLastName, cfg =>
                {
                    cfg.MapFrom(src => src.Requester.AppUser.LastName);
                })
                .ForMember(dest => dest.RequesterEmail, cfg =>
                {
                    cfg.MapFrom(src => src.Requester.AppUser.Email);
                })
                .ForMember(dest => dest.RequesterId, cfg =>
                {
                    cfg.MapFrom(src => src.Requester.Id);
                });
        }
    }
}
