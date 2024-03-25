using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class CustomEvent
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        new ChronosTextEntry("").Encode(byteStream);
        new ChronosTextEntry("").Encode(byteStream);
        new ChronosTextEntry("").Encode(byteStream);
    }
}