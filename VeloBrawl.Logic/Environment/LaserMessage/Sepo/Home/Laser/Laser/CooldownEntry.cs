using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class CooldownEntry
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        ByteStreamHelper.WriteDataReference(byteStream, 0);
        byteStream.WriteVInt(0);
    }
}