﻿using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Input;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Manager;

public class ClientInputManager
{
    private readonly Queue<ClientInput> _inputsQueue = new();

    public void AddInput(ClientInput clientInput)
    {
        _inputsQueue.Enqueue(clientInput);
    }

    public Queue<ClientInput> GetOverrideGroup()
    {
        return _inputsQueue;
    }

    public void ClearList()
    {
        _inputsQueue.Clear();
    }
}