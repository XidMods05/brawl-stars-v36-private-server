using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Database.Alliance;
using VeloBrawl.Logic.Database.IntraSigned;

namespace VeloBrawl.Logic.Database;

public static class Databases
{
    public static AccountDatabase AccountDatabase = null!;
    public static AllianceDatabase AllianceDatabase = null!;
    public static NotificationIntraSignedDatabase NotificationIntraSignedDatabase = null!;
}