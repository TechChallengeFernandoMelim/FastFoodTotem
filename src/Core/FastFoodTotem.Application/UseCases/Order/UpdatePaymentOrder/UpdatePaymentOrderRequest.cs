﻿using MediatR;
using System.Text.Json.Serialization;

namespace FastFoodTotem.Application.UseCases.Order.UpdatePaymentOrder;

public sealed record UpdatePaymentOrderRequest : IRequest<UpdatePaymentOrderResponse>
{
    public string Action { get; set; }

    [JsonPropertyName("api_version")]
    public string? Version { get; set; }
    public OrderPaymentDataDto Data { get; set; }

    [JsonPropertyName("date_created")]
    public DateTime DateCreated { get; set; }
    public long? Id { get; set; }

    [JsonPropertyName("live_mode")]
    public bool? LiveMode { get; set; }
    public string? Type { get; set; }

    [JsonPropertyName("user_id")]
    public string? UserId { get; set; }

    [JsonIgnore]
    public int OrderId { get; set; }
}

public sealed record OrderPaymentDataDto
{
    public string Id { get; set; }
}