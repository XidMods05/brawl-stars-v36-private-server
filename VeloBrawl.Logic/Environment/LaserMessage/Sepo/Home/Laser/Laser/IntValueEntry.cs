using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class IntValueEntry(int x, int y)
{
    public int Key { get; } = x;
    public int Value { get; } = y;

    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteInt(x);
        byteStream.WriteInt(y);
    }
}