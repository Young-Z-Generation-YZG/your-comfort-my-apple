using Microsoft.AspNetCore.Http;

namespace YGZ.Basket.Application.Abstractions.Providers.Momo;

public interface IMomoProvider
{
    Task<MomoCreatePaymentResponseModel> CreatePaymentUrlAsync(MomoInformationModel model);
    MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collections);
}
