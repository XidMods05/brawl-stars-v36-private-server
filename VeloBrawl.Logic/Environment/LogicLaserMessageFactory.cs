using VeloBrawl.Logic.Environment.LaserMessage;
using VeloBrawl.Logic.LHelp;
using VeloBrawl.Supercell.Titan.CommonUtils;
using VeloBrawl.Titan.Graphic;

namespace VeloBrawl.Logic.Environment;

public static class LogicLaserMessageFactory
{
    private static readonly Lazy<Dictionary<int, PiranhaMessage>> LazyMessages;

    static LogicLaserMessageFactory()
    {
        LazyMessages = new Lazy<Dictionary<int, PiranhaMessage>>(() =>
            StaticOnGetService.OnGetMessagesHelper.GetMessages());
    }

    public static PiranhaMessage? CreateMessageByType(int type)
    {
        if (LazyMessages.Value.TryGetValue(type, out var message)) return message;
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Warn,
            $"Unknown message received! Information: type: {type}; name: {Helper.GetPacketNameByType(type)}.");

        return null;
    }
}