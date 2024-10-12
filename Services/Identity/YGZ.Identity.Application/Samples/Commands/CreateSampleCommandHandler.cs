
using System.Windows.Input;
using YGZ.Identity.Application.Common.Abstractions.Messaging;
using YGZ.Identity.Contracts.Samples;
using YGZ.Identity.Domain.Common.Abstractions;
using YGZ.Identity.Domain.Common.Errors;

namespace YGZ.Identity.Application.Samples.Commands;
internal class CreateSampleCommandHandler : ICommandHandler<CreateSampleCommand, CreateSampleResponse>
{
    public async Task<Result<CreateSampleResponse>> Handle(CreateSampleCommand request, CancellationToken cancellationToken)
    {
        
        var response = new CreateSampleResponse(Email: request.Email, ReponseAttribute: "Response Attribute");

        throw new Exception("Sample Exception");

        //return Result<CreateSampleResponse>.Success(response);

        //return Result<CreateSampleResponse>.Failure(SampleErrors.SampleError);
    }
}
