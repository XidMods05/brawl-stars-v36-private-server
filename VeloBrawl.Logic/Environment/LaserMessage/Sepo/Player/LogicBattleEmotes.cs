using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;
using VeloBrawl.Titan.Mathematical.Data;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Player;

public class LogicBattleEmotes
{
    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(5);
        {
            ByteStreamHelper.WriteDataReference(byteStream,
                GlobalId.CreateGlobalId(CsvHelperTable.Emotes.GetId(), 178 - 3));
            ByteStreamHelper.WriteDataReference(byteStream,
                GlobalId.CreateGlobalId(CsvHelperTable.Emotes.GetId(), 148 - 3));
            ByteStreamHelper.WriteDataReference(byteStream,
                GlobalId.CreateGlobalId(CsvHelperTable.Emotes.GetId(), 345 - 3));
            ByteStreamHelper.WriteDataReference(byteStream,
                GlobalId.CreateGlobalId(CsvHelperTable.Emotes.GetId(), 137 - 3));
            ByteStreamHelper.WriteDataReference(byteStream,
                GlobalId.CreateGlobalId(CsvHelperTable.Emotes.GetId(), 67 - 3));
        }
    }
}