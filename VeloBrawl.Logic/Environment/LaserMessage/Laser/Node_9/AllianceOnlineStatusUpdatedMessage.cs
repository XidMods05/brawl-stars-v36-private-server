using VeloBrawl.Logic.Database;
using VeloBrawl.Logic.Database.Alliance;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Alliance;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Alliance.Status;
using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class AllianceOnlineStatusUpdatedMessage(AllianceModel allianceModel) : PiranhaMessage
{
    public AllianceOnlineStatusUpdatedMessage() : this(null!)
    {
        Helper.Skip();
    }

    public required List<AllianceMemberEntry> OnlineMembers { get; set; } = [];
    public required long OwnAccountId { get; set; }

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteVInt((int)OwnAccountId);
        ByteStream.WriteVInt(OnlineMembers.Count);

        foreach (var allianceMemberEntry in OnlineMembers)
        {
            var v1 = new StatusChangeEntry();
            {
                v1.AccountModel = Databases.AccountDatabase.LoadAccount(allianceMemberEntry.AccountId);
            }

            v1.Encode(ByteStream);
        }
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public override int GetMessageType()
    {
        return 20207;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}