using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class LogicPlayerRewardData
{
    public void Encode(ByteStream byteStream)
    {
        new LogicRewardConfig().Encode(byteStream);

        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(false);
    }
}