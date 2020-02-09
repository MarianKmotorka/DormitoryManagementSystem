using System.Linq;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;

namespace Application.Repairers.Queries.GetRepairerDetail
{
    public class RepairerDetail : IMapFrom<Repairer>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string PostCode { get; set; }

        public int NumberOfFixes { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Repairer, RepairerDetail>()
                .ForMember(dest => dest.Email, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.Email);
                })
                .ForMember(dest => dest.FirstName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.FirstName);
                })
                .ForMember(dest => dest.LastName, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.LastName);
                })
                .ForMember(dest => dest.PhoneNumber, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.PhoneNumber);
                })
                .ForMember(dest => dest.Country, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.Address.Country);
                })
                .ForMember(dest => dest.City, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.Address.City);
                })
                .ForMember(dest => dest.Street, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.Address.Street);
                })
                .ForMember(dest => dest.HouseNumber, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.Address.HouseNumber);
                })
                .ForMember(dest => dest.PostCode, cfg =>
                {
                    cfg.MapFrom(src => src.AppUser.Address.PostCode);
                })
                .ForMember(dest => dest.NumberOfFixes, cfg =>
                {
                    cfg.MapFrom(src => src.RepairRequests.Count(x => x.State == RepairRequestState.Fixed));
                });
        }
    }
}
