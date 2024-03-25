namespace VeloBrawl.Titan.Message;

public abstract class TitanLoginMessage
{
    public static int GetMessageType()
    {
        return 10100 + 1;
    }

    public static int GetServiceNodeType()
    {
        return 1;
    }
}