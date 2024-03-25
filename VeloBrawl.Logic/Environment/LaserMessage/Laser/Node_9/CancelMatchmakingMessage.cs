using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class CancelMatchmakingMessage : PiranhaMessage
{
    public CancelMatchmakingMessage()
    {
        Helper.Skip();
    }

    public override int GetMessageType()
    {
        return 14106;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}