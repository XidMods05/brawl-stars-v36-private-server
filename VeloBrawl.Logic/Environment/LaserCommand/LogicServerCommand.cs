using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserCommand;

public class LogicServerCommand(ByteStream byteStream)
{
    public void Decode()
    {
        byteStream.ReadVInt();
        {
            new LogicCommand().Decode(byteStream);
        }
    }

    public void Encode()
    {
        byteStream.WriteVInt(0);
        {
            new LogicCommand().Encode(byteStream);
        }
    }
}