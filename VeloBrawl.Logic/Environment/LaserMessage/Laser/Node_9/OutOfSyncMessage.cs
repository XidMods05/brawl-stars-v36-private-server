using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class OutOfSyncMessage : PiranhaMessage
{
    public OutOfSyncMessage()
    {
        Helper.Skip();
    }

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteVInt(0);
        ByteStream.WriteVInt(0);
        ByteStream.WriteVInt(0);
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public override int GetMessageType()
    {
        return 24104;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}