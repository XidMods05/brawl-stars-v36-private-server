using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class VanityItemEntry(int vanityGlobalId)
{
    public void Encode(ByteStream byteStream)
    {
        ByteStreamHelper.WriteDataReference(byteStream, vanityGlobalId);
        byteStream.WriteVInt(0);
    }
}