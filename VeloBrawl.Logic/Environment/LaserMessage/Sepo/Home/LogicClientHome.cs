using VeloBrawl.Logic.Database;
using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home.Laser;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Home;

public class LogicClientHome(AccountModel accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        var v1 = Databases.NotificationIntraSignedDatabase.GetAppendix(Convert.ToInt64(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(
                AccountStructure.NotificationFactoryIntraSignedId))); // object;

        new LogicDailyData(accountModel).Encode(byteStream);
        new LogicConfData(accountModel).Encode(byteStream);

        byteStream.WriteLong(accountModel.GetAccountId());

        byteStream.WriteVInt(v1.FreeTextNotifications.Count);
        {
            foreach (var notification in v1.FreeTextNotifications)
            {
                byteStream.WriteVInt(notification.Value.GetNotificationType());
                notification.Value.Encode(byteStream);
            }
        }

        byteStream.WriteVInt(-1);
        byteStream.WriteBoolean(false);
        byteStream.WriteVInt(0);

        var v101 = byteStream.WriteVInt(0);
        {
            for (var i = 0; i < v101; i++) ByteStreamHelper.WriteDataReference(byteStream, 0);
        }
    }
}