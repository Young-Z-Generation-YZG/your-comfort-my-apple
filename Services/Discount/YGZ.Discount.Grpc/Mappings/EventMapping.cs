using Mapster;
using YGZ.Discount.Application.Events.Commands.CreateEvent;
using YGZ.Discount.Grpc.Protos;
using Google.Protobuf.WellKnownTypes;

namespace YGZ.Discount.Grpc.Mappings;

public class EventMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // Global mapping for Timestamp to DateTime?
        config.NewConfig<Timestamp, DateTime?>()
            .MapWith(src => src == null ? null : src.ToDateTime());

        config.NewConfig<CreateEventRequest, CreateEventCommand>()
            .Map(dest => dest.Title, src => src.Title)
            .Map(dest => dest.Description, src => src.Description)
            .Map(dest => dest.StartDate, src => src.StartDate)
            .Map(dest => dest.EndDate, src => src.EndDate);
    }
}
