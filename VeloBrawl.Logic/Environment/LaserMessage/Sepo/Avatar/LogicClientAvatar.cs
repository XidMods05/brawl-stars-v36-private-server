using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;
using VeloBrawl.Titan.Mathematical;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Avatar;

public class LogicClientAvatar(AccountModel accountModel)
{
    public void Encode(ByteStream byteStream)
    {
        ByteStreamHelper.EncodeLogicLong(byteStream, new LogicLong(0, Convert.ToInt32(accountModel.GetAccountId())));
        ByteStreamHelper.EncodeLogicLong(byteStream, new LogicLong(0, Convert.ToInt32(accountModel.GetAccountId())));
        ByteStreamHelper.EncodeLogicLong(byteStream, new LogicLong(0,
            Convert.ToInt16(
                accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.HomeId))));

        byteStream.WriteStringReference(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.AvatarName)
                .ToString()!);
        byteStream.WriteBoolean(
            (bool)accountModel
                .GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.NameSetByUser) ||
            !(bool)accountModel
                .GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.UserIsAuthed));

        byteStream.WriteInt(0);

        byteStream.WriteVInt(8);

        byteStream.WriteVInt(2 + 1);

        ByteStreamHelper.WriteDataReference(byteStream, 23, 0);
        byteStream.WriteVInt(1);

        ByteStreamHelper.WriteDataReference(byteStream, 5, 8);
        byteStream.WriteVInt(0);

        ByteStreamHelper.WriteDataReference(byteStream, 5, 10);
        byteStream.WriteVInt(0);

        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(0);
        byteStream.WriteVInt(2);
    }
}