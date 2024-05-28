using System;

namespace QuestSystem.Application.Common.Exceptions;

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base() { }
}
