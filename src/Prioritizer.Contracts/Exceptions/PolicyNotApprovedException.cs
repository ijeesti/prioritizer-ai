namespace Prioritizer.Contracts.Exceptions;

public class PolicyNotApprovedException(string message) : Exception(message)
{
}