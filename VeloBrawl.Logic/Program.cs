using VeloBrawl.Logic.LHelp;
using VeloBrawl.Titan.Graphic;

namespace VeloBrawl.Logic;

public static class Program
{
    public static void Main(string[] args)
    {
        ConsoleLogger.WriteTextWithPrefix(ConsoleLogger.Prefixes.Cmd, "Emulation of 'Logic' has been launched.");
        {
            StaticOnGetService.OnGetMessagesHelper = new OnGetMessagesHelper();
            StaticOnGetService.OnGetMessagesHelper.OnLoad();
        }
    }
}