using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Titan.DataStream;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Alliance.Status;

public class StatusChangeEntry
{
    public AccountModel AccountModel { get; set; } = null!;

    public void Encode(ByteStream byteStream)
    {
        byteStream.WriteLong(AccountModel.GetAccountId());
        byteStream.WriteVInt(
            AccountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.PlayerStatus), true);
    }
}