using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Input;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Manager;
using VeloBrawl.Supercell.Titan.CommonUtils;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_27;

public class ClientInputMessage : PiranhaMessage
{
    public ClientInputMessage()
    {
        Helper.Skip();
    }

    public ClientInputManager? ClientInputManager { get; } = new();

    public override void Decode()
    {
        base.Decode();

        var bitStream = new BitStream(ByteStream.GetByteArray(), ByteStream.GetLength());

        bitStream.ReadPositiveIntMax16383();
        bitStream.ReadPositiveIntMax1023();
        bitStream.ReadPositiveIntMax8191();
        bitStream.ReadPositiveIntMax1023();
        bitStream.ReadPositiveIntMax1023();
        var count = bitStream.ReadPositiveIntMax31();

        for (var i = 0; i < count; i++)
        {
            var clientInput = new ClientInput();
            {
                clientInput.Decode(bitStream);
            }

            ClientInputManager!.AddInput(clientInput);
        }
    }

    public override void Clear()
    {
        base.Clear();
    }

    public override int GetMessageType()
    {
        return 10555;
    }

    public override int GetServiceNodeType()
    {
        return 27;
    }
}