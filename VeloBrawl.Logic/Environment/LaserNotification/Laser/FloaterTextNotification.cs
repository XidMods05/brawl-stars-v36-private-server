using VeloBrawl.Logic.Environment.LaserNotification.Laser.Own;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserNotification.Laser;

public class FloaterTextNotification(string text) : BaseNotification(text, false, -1)
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
        return Type = 66;
    }
}