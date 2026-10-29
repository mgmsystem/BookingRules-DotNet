using BookingRules.Domain;

namespace BookingRules.UnitTests;

[TestClass]
public sealed class ReservationTests
{
    [TestMethod]
    public void DoesNotConfirmForInactiveCustomer()
    {
        var reservation = CreateReservation(customerActive: false);

        Assert.IsFalse(reservation.CanConfirm(1));
    }

    [TestMethod]
    public void DoesNotConfirmForBlockedCustomer()
    {
        var reservation = CreateReservation(
            customerActive: true,
            customerBlocked: true);

        Assert.IsFalse(reservation.CanConfirm(1));
    }

    [TestMethod]
    public void DoesNotConfirmWithoutRepository()
    {
        var reservation = new Reservation(customerId: 1, customerRepository: null);

        Assert.IsFalse(reservation.CanConfirm(1));
    }

    [TestMethod]
    public void QueriesTheReservationCustomer()
    {
        var repository = new FakeCustomerRepository(customerActive: true);
        var reservation = new Reservation(customerId: 25, repository);

        Assert.IsTrue(reservation.CanConfirm(1));
        Assert.AreEqual(25, repository.LastCustomerId);
    }

    [TestMethod]
    public void PropagatesRepositoryFailure()
    {
        var reservation = new Reservation(
            customerId: 1,
            new FailingCustomerRepository());

        Assert.Throws<InvalidOperationException>(() => reservation.CanConfirm(1));
    }

    [TestMethod]
    public void DoesNotQueryRepositoryForInvalidParticipantCount()
    {
        var repository = new SpyCustomerRepository();
        var reservation = new Reservation(customerId: 1, repository);

        Assert.IsFalse(reservation.CanConfirm(0));
        Assert.AreEqual(0, repository.StatusCalls);
    }

    [TestMethod]
    public void QueriesRepositoryOnceForValidReservation()
    {
        var repository = new SpyCustomerRepository();
        var reservation = new Reservation(customerId: 1, repository);

        Assert.IsTrue(reservation.CanConfirm(1));
        Assert.AreEqual(1, repository.StatusCalls);
    }

    [TestMethod]
    public void ReportsBasicRefusalReasons()
    {
        var reservation = CreateReservation(customerActive: true);
        Assert.AreEqual(
            ReservationDecision.InvalidParticipantCount,
            reservation.Evaluate(0));

        reservation = new Reservation(customerId: 1, customerRepository: null);
        Assert.AreEqual(
            ReservationDecision.RepositoryUnavailable,
            reservation.Evaluate(1));
    }

    [TestMethod]
    [DataRow(0, false)]
    [DataRow(-1, false)]
    [DataRow(1, true)]
    [DataRow(5, true)]
    [DataRow(6, false)]
    public void ValidatesParticipantCount(int participantCount, bool expected)
    {
        var reservation = CreateReservation(customerActive: true);

        Assert.AreEqual(expected, reservation.CanConfirm(participantCount));
    }

    private static Reservation CreateReservation(
        bool customerActive,
        bool customerBlocked = false) =>
        new(
            customerId: 1,
            new FakeCustomerRepository(customerActive, customerBlocked));
}
