﻿using System.Reflection;
using VeloBrawl.Logic.Environment.LaserMessage;

namespace VeloBrawl.Logic.LHelp;

public class OnGetMessagesHelper
{
    private readonly Dictionary<int, PiranhaMessage> _messages = new();

    public void OnLoad()
    {
        var messageTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsSubclassOf(typeof(PiranhaMessage)));

        foreach (var messageType in messageTypes)
            if (Activator.CreateInstance(messageType) is PiranhaMessage instance && instance.IsClientToServerMessage())
                _messages.Add(instance.GetMessageType(), instance);
    }

    public Dictionary<int, PiranhaMessage> GetMessages()
    {
        return _messages;
    }
}