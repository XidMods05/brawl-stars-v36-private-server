using VeloBrawl.Logic.Database;
using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Environment.LaserListener;
using VeloBrawl.Logic.Environment.LaserMessage.Sepo.Player;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.Utilities;

namespace VeloBrawl.Logic.Environment.LaserMessage.Sepo.Alliance;

public class AllianceMemberEntry
{
    public long AccountId { get; set; }
    public AllianceRoleHelperTable AllianceRoleHelperTable { get; set; }

    public void Encode(ByteStream byteStream, long myAccountId = -1)
    {
        var accountModel = Databases.AccountDatabase.LoadAccount(AccountId);
        var isOnline = IdentifierListener.GetV2LogicGameListenerByAccountId(accountModel.GetAccountId());

        byteStream.WriteLong(accountModel.GetAccountId());
        byteStream.WriteVInt(Convert.ToInt32(AllianceRoleHelperTable));
        byteStream.WriteVInt(Convert.ToInt32(
            accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.Trophies)));
        byteStream.WriteVInt(isOnline
            ? Convert.ToInt32(
                accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.PlayerStatus))
            : 0);
        byteStream.WriteVInt(isOnline
            ? -1
            : LogicTimeUtil.GetTimestamp() -
              Convert.ToInt32(accountModel.GetFieldValueByAccountStructureParameterFromAccountModel
                  (AccountStructure.LastOnlineTime)));
        byteStream.WriteVInt(0); // power league rank
        byteStream.WriteBoolean(AccountId == myAccountId); // mute invites

        var playerDisplayData = new PlayerDisplayData();
        {
            playerDisplayData.SetAvatarName(accountModel
                .GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure.AvatarName)
                .ToString()!);
            playerDisplayData.SetPlayerThumbnail(Convert.ToInt32(
                accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(AccountStructure
                    .PlayerThumbnailGlobalId)));
            playerDisplayData.SetNameColor(Convert.ToInt32(
                accountModel.GetFieldValueByAccountStructureParameterFromAccountModel(
                    AccountStructure.NameColorGlobalId)));
        }

        playerDisplayData.Encode(byteStream);
    }
}