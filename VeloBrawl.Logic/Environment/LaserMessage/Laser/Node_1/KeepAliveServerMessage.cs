using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_1;

public class KeepAliveServerMessage : PiranhaMessage
{
    public KeepAliveServerMessage()
    {
        Helper.Skip();
    }

    public override int GetMessageType()
    {
        return 20108;
    }

    public override int GetServiceNodeType()
    {
        return 1;
    }
}