namespace BookingRules.Domain;

public enum ReservationDecision
{
    Confirmed,
    InvalidParticipantCount,
    RepositoryUnavailable,
    CustomerNotFound,
    CustomerInactive,
    CustomerBlocked
}
