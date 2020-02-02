using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Officers.Queries.GetOfficerDetail
{
    public class OfficerDetail : IMapFrom<Officer>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string OfficeNumber { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string HouseNumber { get; set; }

        public string PostCode { get; set; }

        public string IdCardNumber { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Officer, OfficerDetail>()
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
                });
        }
    }
}
