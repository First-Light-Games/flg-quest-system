﻿using System;

namespace QuestSystem.Application.Common.Exceptions;

public class ExternalServiceException : Exception
{
    public ExternalServiceException(string message) : base(message) { }
}
