using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Avatar;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home;
using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class OwnHomeDataMessage(AccountModel accountModel) : PiranhaMessage
{
    public OwnHomeDataMessage() : this(new AccountModel())
    {
        Helper.Skip();
    }

    public override void Encode()
    {
        base.Encode();

        new LogicClientHome(accountModel).Encode(ByteStream);
        new LogicClientAvatar(accountModel).Encode(ByteStream);
        ByteStream.WriteVInt(0); // currentTimeInSecondsSinceEpoch
    }

    public override int GetMessageType()
    {
        return 24101;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}