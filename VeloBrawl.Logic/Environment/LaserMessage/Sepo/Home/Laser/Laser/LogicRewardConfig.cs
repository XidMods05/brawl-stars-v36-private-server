using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.Mathematical.Data;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class LogicRewardConfig
{
    public void Encode(ByteStream byteStream)
    {
        new LogicCondition().Encode(byteStream);

        var c = new LogicGemOffer(ShopItemHelperTable.Coin, 1);
        {
            c.SetItem(GlobalId.CreateGlobalId(16, 0), true);
        }

        c.Encode(byteStream);
    }
}