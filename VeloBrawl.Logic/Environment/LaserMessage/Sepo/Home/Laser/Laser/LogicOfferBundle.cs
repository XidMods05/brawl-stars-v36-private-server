﻿using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.Mathematical;
using VeloBrawl.Titan.Utilities;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;

public class LogicOfferBundle(
    List<LogicGemOffer> logicGemOffers,
    string offerTitle,
    string backgroundTheme,
    ShopPriceTypeHelperTable shopPriceTypeHelperTable,
    int offerPrice,
    int oldOfferPrice,
    int endTime,
    bool isInDailyOffers,
    bool confirmPurchase)
{
    private bool _purchased;

    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteVInt(logicGemOffers.Count);
        {
            foreach (var logicGemOffer in logicGemOffers) logicGemOffer.Encode(byteStream);
        }

        byteStream.WriteVInt((int)shopPriceTypeHelperTable);
        byteStream.WriteVInt(offerPrice);

        byteStream.WriteVInt(LogicMath.Max(LogicTimeUtil.GetTimestamp() - endTime, 0));
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(_purchased);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteBoolean(isInDailyOffers);
        byteStream.WriteVInt(oldOfferPrice);

        new ChronosTextEntry(offerTitle).Encode(byteStream);

        byteStream.WriteVInt(confirmPurchase);
        byteStream.WriteString(backgroundTheme);
        byteStream.WriteVInt(0);
        byteStream.WriteBoolean(false);

        if (byteStream.WriteVInt(LogicMath.Abs(oldOfferPrice - offerPrice) % 10 == 0 ? 1 : 2) == 1)
            byteStream.WriteVInt(LogicMath.Max(oldOfferPrice, offerPrice) / LogicMath.Min(oldOfferPrice, offerPrice));
        else
            byteStream.WriteVInt(
                LogicMath.Abs((int)Math.Ceiling((double)(offerPrice - oldOfferPrice) / oldOfferPrice * 100)));
    }

    public bool GetPurchased()
    {
        return _purchased;
    }

    public void SetPurchased(bool value)
    {
        _purchased = value;
    }
}