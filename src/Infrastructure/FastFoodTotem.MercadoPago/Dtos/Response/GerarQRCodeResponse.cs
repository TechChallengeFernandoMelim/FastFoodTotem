using Newtonsoft.Json;

namespace FastFoodTotem.MercadoPago.Dtos.Response;

public class GerarQRCodeResponse
{
    [JsonProperty("qr_data")]
    public string QrData { get; set; }

    [JsonProperty("in_store_order_id")]
    public string InStoreOrderId { get; set; }
}
