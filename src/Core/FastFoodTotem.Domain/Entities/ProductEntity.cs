﻿using FastFoodTotem.Domain.Enums;

namespace FastFoodTotem.Domain.Entities
{
    public class ProductEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CategoryType Type { get; set; }
        public decimal Price { get; set; }

        public IEnumerable<OrderedItemEntity> OrderedItems { get; set; }
    }
}
