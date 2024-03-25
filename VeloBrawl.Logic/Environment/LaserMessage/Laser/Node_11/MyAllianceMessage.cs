using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Alliance;
using VeloBrawl.Supercell.Titan.CommonUtils;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream.Helps;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_11;

public class MyAllianceMessage : PiranhaMessage
{
    public MyAllianceMessage()
    {
        Helper.Skip();
    }

    public int OnlineMembers { get; set; }
    public AllianceRoleHelperTable AllianceRole { get; set; }
    public AllianceHeaderEntry AllianceHeaderEntry { get; set; } = null!;

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteVInt(OnlineMembers);
        if (!ByteStream.WriteBoolean(AllianceHeaderEntry != null!)) return;
        ByteStreamHelper.WriteDataReference(ByteStream, ((LogicAllianceRoleData)LogicDataTables
            .GetDataFromTable(CsvHelperTable.AllianceRoles.GetId())
            .GetDataByName(AllianceRole.GetCsvName())).GetGlobalId());
        AllianceHeaderEntry!.Encode(ByteStream);
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public override int GetMessageType()
    {
        return 24399;
    }

    public override int GetServiceNodeType()
    {
        return 11;
    }
}