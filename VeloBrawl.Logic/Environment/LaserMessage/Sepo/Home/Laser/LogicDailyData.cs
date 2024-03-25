using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser.Laser;
using VeloBrawl.Supercell.Titan.CommonUtils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;
using VeloBrawl.Titan.Mathematical;
using VeloBrawl.Titan.Mathematical.Data;
using VeloBrawl.Titan.Utilities;
using VeloBrawl.Tools.LaserFactory;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser;

public class LogicDailyData(AccountModel accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        var v1 = (List<LogicOfferBundle>)accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(
            AccountStructure.LogicOfferBundleList); // object;
        var v2 = (LanguageFactory)accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure
            .LanguageLaserFactory); // object;
        var v3 = (Dictionary<int, IntValueEntry>)accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(
            AccountStructure.IntValueEntryX); // object;

        byteStream.WriteVInt(Helper.GetCurrentTimeInSecondsSinceEpoch());
        byteStream.WriteVInt(Helper.GetCurrentTimeInSecondsSinceEpoch());

        byteStream.WriteVInt(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.Trophies), true);
        byteStream.WriteVInt(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.MaxTrophies), true);
        byteStream.WriteVInt(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.MaxTrophies), true);
        byteStream.WriteVInt(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.TrophyRoadProgress),
            true);
        byteStream.WriteVInt(999);

        ByteStreamHelper.WriteDataReference(byteStream,
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure
                .PlayerThumbnailGlobalId));
        ByteStreamHelper.WriteDataReference(byteStream,
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.NameColorGlobalId));

        var v101 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v101; i++) byteStream.WriteVInt(i);
        }

        var v102 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v102; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        var v103 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v103; i++)
            {
                byteStream.WriteVInt(i);
                ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
            }
        }

        var v104 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v104; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        var v105 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v105; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        var v106 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v106; i++) ByteStreamHelper.WriteDataReference(byteStream, 0, 0);
        }

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.MaxTrophies), true);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteBoolean(true);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        new ForcedDrops().Encode(byteStream);

        if (byteStream.WriteBoolean(false))
            new TimedOffer().Encode(byteStream);

        if (byteStream.WriteBoolean(false))
            new TimedOffer().Encode(byteStream);

        byteStream.WriteBoolean(false);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        byteStream.WriteVInt(Helper.GetChangeNameCostByCount(Convert.ToInt32(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.NameChangeCount))));
        byteStream.WriteVInt(LogicMath.Max(
            Convert.ToInt32(
                accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(
                    AccountStructure.NameChangeEndTime)) - LogicTimeUtil.GetTimestamp(), 0));
        byteStream.WriteVInt(0);

        byteStream.WriteVInt(v1.Count);
        {
            foreach (var v1L in v1) v1L.Encode(byteStream);
        }

        var v107 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v107; i++) new AdStatus().Encode(byteStream);
        }

        byteStream.WriteVInt(0); // available battle tokens;
        byteStream.WriteVInt(-1);

        var v108 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v108; i++) byteStream.WriteVInt(i);
        }

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);

        ByteStreamHelper.WriteDataReference(byteStream, GlobalId.CreateGlobalId(16, 0));

        byteStream.WriteString(v2.GetTextCodeByLanguage().ToUpper());
        byteStream.WriteString(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.SupportedCreator),
            true);

        var v109 = byteStream.WriteVInt(v3.Count);
        {
            if (v109 > 0)
                foreach (var v109L in v3)
                    v109L.Value.Encode(byteStream);
        }

        var v110 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v110; i++) new CooldownEntry().Encode(byteStream);
        }

        var v111 = byteStream.WriteVInt(1);
        {
            for (var i = 0; i < v111; i++) new BrawlPassSeasonData(accountModel).Encode(byteStream);
        }

        var v112 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v112; i++) new ProLeagueSeasonData().Encode(byteStream);
        }

        if (byteStream.WriteBoolean(true)) new LogicQuests([]).Encode(byteStream);

        if (byteStream.WriteBoolean(true)) new VanityItems([]).Encode(byteStream);

        if (byteStream.WriteBoolean(true)) new LogicPlayerRankedSeasonData().Encode(byteStream);

        byteStream.WriteInt(0);
    }
}