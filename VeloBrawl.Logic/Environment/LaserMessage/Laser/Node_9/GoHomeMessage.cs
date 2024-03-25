using VeloBrawl.Supercell.Titan.CommonUtils;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;

public class GoHomeMessage : PiranhaMessage
{
    private bool _clearActiveBattle;

    public GoHomeMessage()
    {
        Helper.Skip();
    }

    public override void Decode()
    {
        base.Decode();

        _clearActiveBattle = ByteStream.ReadBoolean();
        _ = ByteStream.ReadBoolean();
    }

    public override void Clear()
    {
        base.Clear();
    }

    public bool GetClearActiveBattle()
    {
        return _clearActiveBattle;
    }

    public void SetClearActiveBattle(bool clearActiveBattle)
    {
        _clearActiveBattle = clearActiveBattle;
    }

    public override int GetMessageType()
    {
        return 14101;
    }

    public override int GetServiceNodeType()
    {
        return 9;
    }
}