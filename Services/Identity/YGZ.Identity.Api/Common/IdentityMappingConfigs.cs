using Mapster;
using YGZ.Identity.Api.Contracts;
using YGZ.Identity.Application.Auths.Commands.Login;
using YGZ.Identity.Application.Auths.Commands.Register;

namespace YGZ.Identity.Api.Common;

public class IdentityMappingConfigs : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        //config.NewConfig<RegisterRequest, RegisterCommand>()
        //    .Map(dest => dest.FullName, src => src.Full_name);
    }
}
