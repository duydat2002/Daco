namespace Daco.Infrastructure.Persistence.Repositories.Orders
{
    public class OrderRepository : IOrderRepository
    {
        public Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetByOrderCodeAsync(string orderCode, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
