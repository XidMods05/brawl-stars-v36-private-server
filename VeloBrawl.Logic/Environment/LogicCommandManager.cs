using VeloBrawl.Logic.Environment.LaserCommand;
using VeloBrawl.Logic.Environment.LaserCommand.Laser;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment;

public abstract class LogicCommandManager
{
    private static readonly Dictionary<int, Type> CommandTypes = new()
    {
        { new LogicSetPlayerAgeCommand().GetCommandType(), typeof(LogicSetPlayerAgeCommand) }
    };

    public static LogicCommand? DecodeCommand(ByteStream byteStream)
    {
        LogicCommand? command = null!;
        var type = byteStream.ReadVInt();

        try
        {
            command = CreateCommand(type);
        }
        catch
        {
            // ignored.
        }

        if (command == null) return null;
        command.Decode(byteStream);
        return command;
    }

    public static void EncodeCommand(LogicCommand logicCommand, ByteStream byteStream)
    {
        byteStream.WriteVInt(logicCommand.GetCommandType());
        {
            logicCommand.Encode(byteStream);
        }
    }

    public static LogicCommand? CreateCommand(int type)
    {
        if (CommandTypes.TryGetValue(type, out var commandType))
            return (LogicCommand)Activator.CreateInstance(commandType)!;

        return null;
    }
}