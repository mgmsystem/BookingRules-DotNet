using BookingRules.Domain;
using BookingRules.Infrastructure;

var repository = new InMemoryCustomerRepository();
repository.AddCustomer(1, active: true, blocked: false);
repository.AddCustomer(2, active: false, blocked: false);
repository.AddCustomer(3, active: true, blocked: true);

Console.WriteLine("========================================");
Console.WriteLine(" BookingRules - Rule Demonstration");
Console.WriteLine("========================================");
Console.WriteLine();

DemonstrateReservation(repository, customerId: 1, participantCount: 3);
DemonstrateReservation(repository, customerId: 1, participantCount: 6);
DemonstrateReservation(repository, customerId: 2, participantCount: 2);
DemonstrateReservation(repository, customerId: 3, participantCount: 2);
DemonstrateReservation(repository, customerId: 99, participantCount: 2);

static void DemonstrateReservation(
    ICustomerRepository repository,
    int customerId,
    int participantCount)
{
    var reservation = new Reservation(customerId, repository);
    var decision = reservation.Evaluate(participantCount);

    Console.WriteLine(
        $"Customer: {customerId} | Participants: {participantCount} | " +
        $"Result: {DecisionToText(decision)}");
}

static string DecisionToText(ReservationDecision decision) => decision switch
{
    ReservationDecision.Confirmed => "CONFIRMED",
    ReservationDecision.InvalidParticipantCount =>
        "REFUSED - invalid participant count",
    ReservationDecision.RepositoryUnavailable =>
        "REFUSED - repository unavailable",
    ReservationDecision.CustomerNotFound => "REFUSED - customer not found",
    ReservationDecision.CustomerInactive => "REFUSED - customer inactive",
    ReservationDecision.CustomerBlocked => "REFUSED - customer blocked",
    _ => "REFUSED - unknown reason"
};
