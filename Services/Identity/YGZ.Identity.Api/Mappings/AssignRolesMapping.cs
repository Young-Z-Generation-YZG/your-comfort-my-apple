using Mapster;
using YGZ.Identity.Api.Contracts.Auth;
using YGZ.Identity.Application.Auths.Commands.AssignRoles;

namespace YGZ.Identity.Api.Mappings;

public class AssignRolesMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AssignRolesRequest, AssignRolesCommand>()
            .Map(dest => dest.UserId, src => src.UserId)
            .Map(dest => dest.Roles, src => src.Roles);
    }
}

