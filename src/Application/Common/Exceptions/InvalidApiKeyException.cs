using System;

namespace QuestSystem.Application.Common.Exceptions;

public class InvalidApiKeyException : Exception
{
    public InvalidApiKeyException(string message) : base(message) { }
}
