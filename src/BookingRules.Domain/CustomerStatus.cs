namespace BookingRules.Domain;

public readonly record struct CustomerStatus(bool Active, bool Blocked);
