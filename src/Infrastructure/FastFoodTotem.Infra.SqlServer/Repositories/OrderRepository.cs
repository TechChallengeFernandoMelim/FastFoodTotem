using FastFoodTotem.Domain.Contracts.Repositories;
using FastFoodTotem.Domain.Entities;
using FastFoodTotem.Infra.SqlServer.Database;
using Microsoft.EntityFrameworkCore;

namespace FastFoodTotem.Infra.SqlServer.Repositories
{
    public class OrderRepository : BaseRepository<int, OrderEntity>, IOrderRepository
    {
        public OrderRepository(FastFoodContext fastFoodContext) : base(fastFoodContext)
        {
        }

        public async Task AddOrderAsync(OrderEntity order, CancellationToken cancellationToken)
        {
            await CreateAsync(order);
            await SaveChangesAsync(cancellationToken);

            order = await Data.Include(x => x.OrderedItems).ThenInclude(x => x.Product).FirstAsync(x => x.Id == order.Id);
        }

        public async Task EditOrderAsync(OrderEntity order, CancellationToken cancellationToken)
        {
            Update(order);
            await SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync(CancellationToken cancellationToken)
         => await Data
            .Include(x => x.OrderedItems)
            .ThenInclude(x => x.Product)
            .ToListAsync(cancellationToken);

        public async Task<OrderEntity?> GetOrderAsync(int orderId, CancellationToken cancellationToken)
            => await Data.Include(x => x.OrderedItems).ThenInclude(x => x.Product).FirstOrDefaultAsync(x => x.Id == orderId);

        public async Task DeleteUserDataByCpf(string cpf, CancellationToken cancellationToken)
        {
            var userOrders = Data.Where(x => x.UserCpf == cpf);

            foreach (var item in userOrders)
            {
                item.UserCpf = string.Empty;
            }

            await SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteOrderById(int id, CancellationToken cancellationToken)
        {
            var order = await Data.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (order != null)
            {
                Data.Remove(order);
                await SaveChangesAsync(cancellationToken);
            }
        }
    }
}
