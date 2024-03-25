using VeloBrawl.General.Cloud;
using VeloBrawl.Logic.Environment;
using VeloBrawl.Logic.Environment.LaserMessage;
using VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_27;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.General.Networking.LaserUdp;

public class UdpPacketHandler(long sessionId, ByteStream byteStream)
{
    public void Receive()
    {
        var type = byteStream.ReadVInt();
        var length = byteStream.ReadVInt();
        var data = byteStream.ReadBytes(length, 2048);

        var message = LogicLaserMessageFactory.CreateMessageByType(type);
        if (message == null) return;

        message.ByteStream.SetByteArray(data, length);
        message.Decode();

        HandleMessage(message);
    }

    private void HandleMessage(PiranhaMessage piranhaMessage)
    {
        if (piranhaMessage.GetMessageType() != 10555) return;

        while (((ClientInputMessage)piranhaMessage).ClientInputManager!.GetOverrideGroup()
               .TryDequeue(out var clientInput))
        {
            clientInput.OwnSessionId = sessionId;
            InteractiveModule.LogicBattleModeServersMassive![sessionId].HandleClientInput(clientInput);
        }
    }
}