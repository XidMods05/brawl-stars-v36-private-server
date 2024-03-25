using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class ReleaseEntry
{
    public void Encode(ByteStream byteStream)
    {
        ByteStreamHelper.WriteDataReference(byteStream, 0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
    }
}