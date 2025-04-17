using NJsonSchema.Generation;

namespace YGZ.Ordering.Api.Contracts;

public class VnpayIpnCheckRequestExample : ISchemaProcessor
{
    public void Process(SchemaProcessorContext context)
    {
        if (context.ContextualType.Type == typeof(VnpayIpnCheckRequest))
        {
            var schema = context.Schema;

            schema.Example = new
            {
                vnp_Amount = "3595500000",
                vnp_BankCode = "NCB",
                vnp_BankTranNo = "VNP14895090",
                vnp_CardType = "ATM",
                vnp_OrderInfo = "ORDER_ID%3Dcb588ea5-897e-4b3b-b092-e2c533db40e0",
                vnp_PayDate = "20250408140537",
                vnp_ResponseCode = "00",
                vnp_TmnCode = "SB1TO3BK",
                vnp_TransactionNo = "14895090",
                vnp_TransactionStatus = "00",
                vnp_TxnRef = "638796926952539766",
                vnp_SecureHash = "611c8f608a3121b5cfc3396dfec01df31876abab47e7e9b4f29315d29c8805a002fe3cd35e9e09046bda20ee732a1a1806735884d3bfd739faded49e32b0d93c"
            };
        }
    }
}