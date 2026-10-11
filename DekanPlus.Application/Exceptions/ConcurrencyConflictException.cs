namespace DekanPlus.Application.Exceptions;

public class ConcurrencyConflictException(string message) : Exception(message);
