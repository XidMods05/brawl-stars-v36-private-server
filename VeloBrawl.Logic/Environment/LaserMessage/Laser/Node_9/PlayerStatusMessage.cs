using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class PlayerStatusMessage : PiranhaMessage
{
    private int _playerStatus;

    public PlayerStatusMessage()
    {
        Helper.Skip();
    }

    public override void Decode()
    {
        base.Decode();

        _playerStatus = ByteStream.ReadVInt();
    }

    public override void Clear()
    {
        base.Clear();
    }

    public int GetPlayerStatus()
    {
        return _playerStatus;
    }

    public void SetPlayerStatus(int value)
    {
        _playerStatus = value;
    }

    public override int GetMessageType()
    {
        return 14366;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}