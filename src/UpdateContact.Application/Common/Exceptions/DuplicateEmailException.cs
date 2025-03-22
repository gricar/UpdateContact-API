using System.Net;

namespace UpdateContact.Application.Common.Exceptions;

public class DuplicateEmailException : ApplicationException
{
    public DuplicateEmailException(string email)
        : base($"A contact with the same Email '{email}' already exists.",
            HttpStatusCode.Conflict)
    { }
}