using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class ServerErrorMessage(int reason) : PiranhaMessage
{
    public ServerErrorMessage() : this(100)
    {
        Helper.Skip();
    }

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteInt(reason);
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public override int GetMessageType()
    {
        return 24115;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}