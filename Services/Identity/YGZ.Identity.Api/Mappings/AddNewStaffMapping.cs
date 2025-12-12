using Mapster;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.AddNewStaff;

namespace YGZ.Identity.Api.Mappings;

public class AddNewStaffMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AddNewStaffRequest, AddNewStaffCommand>()
            .Map(dest => dest.BirthDay, src => src.BirthDay)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.Password, src => src.Password)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.PhoneNumber, src => src.PhoneNumber)
            .Map(dest => dest.RoleName, src => src.RoleName)
            .Map(dest => dest.TenantId, src => src.TenantId)
            .Map(dest => dest.BranchId, src => src.BranchId);
    }
}

