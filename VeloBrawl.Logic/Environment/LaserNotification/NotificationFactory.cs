﻿using VeloBrawl.Logic.Database;
using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Environment.LaserCommand.Laser;
using VeloBrawl.Logic.Environment.LaserListener;
using VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_9;
using VeloBrawl.Logic.Environment.LaserNotification.Laser;
using VeloBrawl.Logic.Environment.LaserNotification.Laser.Own;

namespace VeloBrawl.Logic.Environment.LaserNotification;

public class NotificationFactory
{
    public readonly Dictionary<int, FloaterTextNotification> FloaterTextNotifications = new();
    public readonly Dictionary<int, FreeTextNotification> FreeTextNotifications = new();
    public AccountModel AccountModel { get; set; } = null!;
    public int NotificationCounter { get; set; } = -1;

    public void Create(BaseNotification baseNotification)
    {
        if (baseNotification.GetNotificationType() > 1) NotificationCounter++;

        var v1 = new AvailableServerCommandMessage();
        {
            v1.SetServerCommand(new LogicAddNotificationCommand(baseNotification));
        }

        if (IdentifierListener.GetV2LogicGameListenerByAccountId(AccountModel.GetAccountId()))
            IdentifierListener.GetLogicGameListenerByAccountId(AccountModel.GetAccountId()).Send(v1);

        switch (baseNotification.GetNotificationType())
        {
            case 66:
                FloaterTextNotifications.Add(NotificationCounter, (baseNotification as FloaterTextNotification)!);
                break;
            case 81:
                FreeTextNotifications.Add(NotificationCounter, (baseNotification as FreeTextNotification)!);
                break;
        }

        Databases.NotificationIntraSignedDatabase.ReplaceAppendix(this);
    }
}