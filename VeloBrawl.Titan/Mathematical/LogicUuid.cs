using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Logic;

namespace VeloBrawl.Titan.Mathematical;

public class LogicUuid
{
    private int _x;
    private int _y;

    public void CopyFrom(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public int Decode(ByteStream byteStream)
    {
        _x = byteStream.ReadVInt();
        _y = byteStream.ReadVInt();

        return _y;
    }

    public int Encode(ChecksumEncoder checksumEncoder)
    {
        checksumEncoder.WriteVInt(_x);
        checksumEncoder.WriteVInt(_y);

        return _y;
    }

    public int Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(_x);
        byteStream.WriteVInt(_y);

        return _y;
    }
}