using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class ProLeagueSeasonData
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
    }
}