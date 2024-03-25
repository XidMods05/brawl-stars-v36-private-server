using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_4;

public class CreateGameFailedMessage : PiranhaMessage
{
    private int _errorCode;

    public CreateGameFailedMessage()
    {
        Helper.Skip();
    }

    public override void Encode()
    {
        base.Encode();

        ByteStream.WriteInt(_errorCode);
    }

    public override void Destruct()
    {
        base.Destruct();
    }

    public int GetErrorCode()
    {
        return _errorCode;
    }

    public void SetErrorCode(int errorCode)
    {
        _errorCode = errorCode;
    }

    public override int GetMessageType()
    {
        return 20402;
    }

    public override int GetServiceNodeType()
    {
        return 4;
    }
}