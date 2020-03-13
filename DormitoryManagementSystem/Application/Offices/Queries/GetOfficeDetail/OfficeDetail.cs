using System.Collections.Generic;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Offices.Queries.GetOfficeDetail
{
    public class OfficeDetail : IMapFrom<Office>
    {
        public int Id { get; set; }

        public string OfficeNumber { get; set; }

        public int FreeTables { get; set; }

        public int Capacity { get; set; }

        public IEnumerable<OfficerDto> Officers { get; set; }

        public class OfficerDto
        {
            public string Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }
        }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Office, OfficeDetail>()
                .ForMember(dest => dest.FreeTables, cfg =>
                {
                    cfg.MapFrom(src => src.Capacity - src.Officers.Count);
                })
                .ForMember(dest => dest.OfficeNumber, cfg =>
                {
                    cfg.MapFrom(src => src.Number);
                });

            profile.CreateMap<Officer, OfficerDto>()
                .ForMember(dest => dest.LastName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName);
                })
                .ForMember(dest => dest.FirstName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.FirstName);
                });
        }
    }
}
