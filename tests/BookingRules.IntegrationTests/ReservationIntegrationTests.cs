using BookingRules.Domain;
using BookingRules.Infrastructure;

namespace BookingRules.IntegrationTests;

[TestClass]
public sealed class ReservationIntegrationTests
{
    [TestMethod]
    public void ConfirmsForActiveCustomer()
    {
        var reservation = CreateReservation(active: true, blocked: false);

        Assert.IsTrue(reservation.CanConfirm(1));
    }

    [TestMethod]
    public void DoesNotConfirmForBlockedCustomer()
    {
        var reservation = CreateReservation(active: true, blocked: true);

        Assert.IsFalse(reservation.CanConfirm(1));
    }

    [TestMethod]
    public void DoesNotConfirmForMissingCustomer()
    {
        var reservation = new Reservation(
            customerId: 99,
            new InMemoryCustomerRepository());

        Assert.IsFalse(reservation.CanConfirm(1));
    }

    [TestMethod]
    public void ReportsInactiveCustomer()
    {
        var reservation = CreateReservation(active: false, blocked: false);

        Assert.AreEqual(
            ReservationDecision.CustomerInactive,
            reservation.Evaluate(1));
    }

    [TestMethod]
    public void ReportsBlockedCustomer()
    {
        var reservation = CreateReservation(active: true, blocked: true);

        Assert.AreEqual(
            ReservationDecision.CustomerBlocked,
            reservation.Evaluate(1));
    }

    [TestMethod]
    public void ReportsMissingCustomer()
    {
        var reservation = new Reservation(
            customerId: 99,
            new InMemoryCustomerRepository());

        Assert.AreEqual(
            ReservationDecision.CustomerNotFound,
            reservation.Evaluate(1));
    }

    private static Reservation CreateReservation(bool active, bool blocked)
    {
        var repository = new InMemoryCustomerRepository();
        repository.AddCustomer(id: 25, active, blocked);
        return new Reservation(customerId: 25, repository);
    }
}
