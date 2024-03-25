using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class LogicPlayerRankedSeasonData
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        var v101 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v101; i++) new LogicPlayerRewardData().Encode(byteStream);
        }
    }
}