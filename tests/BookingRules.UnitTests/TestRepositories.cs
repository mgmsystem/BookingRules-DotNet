using BookingRules.Domain;

namespace BookingRules.UnitTests;

internal sealed class FakeCustomerRepository(
    bool customerActive,
    bool customerBlocked = false) : ICustomerRepository
{
    public int LastCustomerId { get; private set; }

    public bool TryGetStatus(int customerId, out CustomerStatus status)
    {
        LastCustomerId = customerId;
        status = new CustomerStatus(customerActive, customerBlocked);
        return true;
    }
}

internal sealed class SpyCustomerRepository(
    bool customerActive = true) : ICustomerRepository
{
    public int StatusCalls { get; private set; }

    public bool TryGetStatus(int customerId, out CustomerStatus status)
    {
        StatusCalls++;
        status = new CustomerStatus(customerActive, Blocked: false);
        return true;
    }
}

internal sealed class FailingCustomerRepository : ICustomerRepository
{
    public bool TryGetStatus(int customerId, out CustomerStatus status) =>
        throw new InvalidOperationException("Failed to query customer");
}
