using Mapster;
using YGZ.Identity.Api.Contracts.Users;
using YGZ.Identity.Application.Users.Queries.GetUsersByAdmin;

namespace YGZ.Identity.Api.Mappings;

public class GetUsersPaginationMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);

        config.NewConfig<GetUsersPaginationRequest, GetUsersByAdminQuery>()
            .Map(dest => dest.Page, src => src._page)
            .Map(dest => dest.Limit, src => src._limit)
            .Map(dest => dest.Email, src => src._email)
            .Map(dest => dest.FirstName, src => src._firstName)
            .Map(dest => dest.LastName, src => src._lastName)
            .Map(dest => dest.PhoneNumber, src => src._phoneNumber);
    }
}
