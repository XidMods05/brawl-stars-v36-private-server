using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class AvatarNameChangeFailedMessage : PiranhaMessage
{
    private int _reason;

    public AvatarNameChangeFailedMessage()
    {
        Helper.Skip();
    }

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteInt(_reason);
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public int GetReason()
    {
        return _reason;
    }

    public void SetReason(int reason)
    {
        _reason = reason;
    }

    public override int GetMessageType()
    {
        return 20205;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}