using Application.Features.MembershipMovement.Queries.GetMembershipMovementList;
using AutoMapper;
using Domain.Entities;

namespace Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
         CreateMap<MembershipMovement, MembershipMovementDto>().ReverseMap();

    }

}
