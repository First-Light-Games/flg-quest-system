using System;

namespace QuestSystem.Application.Common.Exceptions;

public class PlatformQuestNotCompletedException : Exception
{
    public PlatformQuestNotCompletedException(string message) : base(message) { }
}
