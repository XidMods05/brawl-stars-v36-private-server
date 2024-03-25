using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class BrawlPassSeasonData(AccountModel accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(6);
        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(false);
        byteStream.WriteVInt(0);

        byteStream.WriteByte(2);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);

        byteStream.WriteByte(1);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
        byteStream.WriteInt(0);
    }
}