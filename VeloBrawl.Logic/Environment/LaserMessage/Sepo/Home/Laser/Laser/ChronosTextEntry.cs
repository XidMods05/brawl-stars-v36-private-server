using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class ChronosTextEntry(string text, bool outOfCsv = false)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteInt(outOfCsv ? 1 : 0);
        byteStream.WriteStringReference(text);
    }
}