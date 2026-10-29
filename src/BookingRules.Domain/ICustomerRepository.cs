namespace BookingRules.Domain;

public interface ICustomerRepository
{
    bool TryGetStatus(int customerId, out CustomerStatus status);
}
