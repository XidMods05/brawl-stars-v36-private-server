﻿using VeloBrawl.Logic.Environment.LaserNotification.Laser.Own;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserNotification.Laser;

public class FreeTextNotification(string text, int timestamp) : BaseNotification(text, false, timestamp)
{
    public override void Encode(ByteStream byteStream)
    {
        base.Encode(byteStream);

        byteStream.WriteVInt(1);
    }

    public override void Destruct()
    {
        // not now.
    }

    public override int GetNotificationType()
    {
        return Type = 81;
    }
}