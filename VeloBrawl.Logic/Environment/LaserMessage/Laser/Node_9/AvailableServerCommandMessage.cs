using VeloBrawl.Logic.Environment.LaserCommand;
using VeloBrawl.Supercell.Titan.CommonUtils;
using VeloBrawl.Titan.Graphic;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class AvailableServerCommandMessage : PiranhaMessage
{
    private LogicCommand _logicCommand = null!;

    public AvailableServerCommandMessage()
    {
        Helper.Skip();
    }

    public override void Encode()
    {
        base.Encode();

        LogicCommandManager.EncodeCommand(_logicCommand, ByteStream);
    }

    public void SetServerCommand(LogicCommand logicCommand)
    {
        _logicCommand = logicCommand;
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd,
            $"Set command received! {logicCommand.GetType().Name}.");
    }

    public override int GetMessageType()
    {
        return 24111;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}