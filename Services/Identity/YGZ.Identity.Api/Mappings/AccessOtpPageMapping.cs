using Mapster;
using YGZ.Identity.Api.Contracts;
using YGZ.Identity.Application.Auths.Commands.AccessOtpPage;

namespace YGZ.Identity.Api.Mappings;

public class AccessOtpPageMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<AccessOtpRequest, AccessOtpPageCommand>()
            .Map(dest => dest.Email, src => src._email)
            .Map(dest => dest.VerifyType, src => src._verifyType)
            .Map(dest => dest.Token, src => src._token);
    }
}
