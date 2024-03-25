using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Player;

public class LogicHeroUpgrades
{
    public void Encode(ByteStream byteStream, List<LogicCardData> cardInfo)
    {
        byteStream.WriteVInt(0);
        ByteStreamHelper.WriteDataReference(byteStream, cardInfo[0].GetGlobalId());
        ByteStreamHelper.WriteDataReference(byteStream, cardInfo[1].GetGlobalId());
    }
}