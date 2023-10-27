﻿namespace FastFoodTotem.Application.Dtos.Responses.Order
{
    public class OrderItemResponseDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
    }
}
