using ATOH.Application.Common.Mappings;
using ATOH.Domain.Models;
using AutoMapper;

namespace ATOH.Application.Users;

public class UserLookupDto : IMapWith<User>
{
    public string Name { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateTime BirthDay { get; set; }

    public bool IsActive { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<User, UserLookupDto>()
            .ForMember(dto => dto.Name, opt =>
            {
                opt.MapFrom(u => u.Name);
            })
            .ForMember(dto => dto.Gender, opt =>
            {
                opt.MapFrom(u => u.Gender);
            })
            .ForMember(dto => dto.BirthDay, opt =>
            {
                opt.MapFrom(u => u.BirthDay);
            })
            .ForMember(dto => dto.IsActive, opt =>
            {
                opt.MapFrom(u => u.RevokedOn == null);
            });
    }
}