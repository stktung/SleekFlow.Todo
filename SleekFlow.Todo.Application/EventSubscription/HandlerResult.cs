﻿using EventStore.Core.Data;

namespace SleekFlow.Todo.Application.EventSubscription;

public class HandlerResult
{
    public bool IsSuccess;
    public Error? Error;
}

public class Error
{
    public string Code;
    public string Message;

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}

public static class Errors
{
    public static Error InvalidEventLink(string link)
        => new Error(nameof(InvalidEventLink), $"Invalid event link: '{link}'");

    public static Error SkipEvent<T>(string reason) where T : Event
        => new Error(nameof(SkipEvent), $"Event '{typeof(T).Name}' should be skipped. Reason: '{reason}'.");

    public static Error RetryEvent<T>(string reason) where T : Event
        => new Error(nameof(RetryEvent), $"Event '{typeof(T).Name}' is to be retried. Reason: '{reason}'.");
}