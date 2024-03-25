using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo;

public class AnalyticEvent(ByteStream byteStream)
{
    private string _event = null!;
    private string _eventInfo = null!;

    public void Decode()
    {
        _event = byteStream.ReadString(1024);
        _eventInfo = byteStream.ReadString(1024);
    }

    public string GetEvent()
    {
        return _event;
    }

    public string GetEventInfo()
    {
        return _eventInfo;
    }
}