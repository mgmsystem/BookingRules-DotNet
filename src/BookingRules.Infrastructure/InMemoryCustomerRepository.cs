using BookingRules.Domain;

namespace BookingRules.Infrastructure;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly Dictionary<int, CustomerStatus> _customers = [];

    public void AddCustomer(int id, bool active, bool blocked) =>
        _customers[id] = new CustomerStatus(active, blocked);

    public bool TryGetStatus(int customerId, out CustomerStatus status) =>
        _customers.TryGetValue(customerId, out status);
}
