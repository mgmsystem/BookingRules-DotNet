namespace BookingRules.Domain;

public sealed class Reservation(
    int customerId,
    ICustomerRepository? customerRepository)
{
    private const int MaximumParticipantCount = 5;

    public ReservationDecision Evaluate(int participantCount)
    {
        if (participantCount is <= 0 or > MaximumParticipantCount)
        {
            return ReservationDecision.InvalidParticipantCount;
        }

        if (customerRepository is null)
        {
            return ReservationDecision.RepositoryUnavailable;
        }

        if (!customerRepository.TryGetStatus(customerId, out var status))
        {
            return ReservationDecision.CustomerNotFound;
        }

        if (!status.Active)
        {
            return ReservationDecision.CustomerInactive;
        }

        return status.Blocked
            ? ReservationDecision.CustomerBlocked
            : ReservationDecision.Confirmed;
    }

    public bool CanConfirm(int participantCount) =>
        Evaluate(participantCount) is ReservationDecision.Confirmed;
}
