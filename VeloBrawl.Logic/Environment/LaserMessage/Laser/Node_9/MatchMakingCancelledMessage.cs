using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class MatchMakingCancelledMessage : PiranhaMessage
{
    public MatchMakingCancelledMessage()
    {
        Helper.Skip();
    }

    public override int GetMessageType()
    {
        return 20406;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}