using FastFoodTotem.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FastFoodTotem.Domain.Entities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }
        public string? UserCpf { get; set; }
        public string? UserName { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        public IEnumerable<OrderedItemEntity> OrderedItems { get; set; }

        public string? InStoreOrderId { get; set; }

        public decimal GetTotal()
        => OrderedItems.Sum(item => item.GetTotal());
    }
}
