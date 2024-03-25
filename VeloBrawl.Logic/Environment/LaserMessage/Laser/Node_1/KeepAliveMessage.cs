using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_1;

public class KeepAliveMessage : PiranhaMessage
{
    public KeepAliveMessage()
    {
        Helper.Skip();
    }

    public override int GetMessageType()
    {
        return 10108;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}