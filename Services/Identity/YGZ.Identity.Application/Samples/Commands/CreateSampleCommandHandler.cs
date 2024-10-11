
using System.Windows.Input;
using YGZ.Identity.Application.Common.Abstractions.Messaging;
using YGZ.Identity.Contracts.Samples;
using YGZ.Identity.Domain.Common.Abstractions;

namespace YGZ.Identity.Application.Samples.Commands;
internal class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
{
    public Task<Result<CreateSampleResponse>> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
    {
        
        var response = new CreateSampleResponse(Email: request.Email, ReponseAttribute: "Response Attribute");

        throw new Exception("Sample Exception");
    }
}
