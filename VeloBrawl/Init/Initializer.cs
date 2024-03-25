using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json.Linq;
using VeloBrawl.General.Cloud;
using VeloBrawl.General.NetIsland;
using VeloBrawl.Logic.Database;
using VeloBrawl.Logic.Database.Account;
using VeloBrawl.Logic.Database.Alliance;
using VeloBrawl.Logic.Database.IntraSigned;
using VeloBrawl.Logic.Dynamic;
using VeloBrawl.StaticService.Laser;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Supercell.Titan.CommonUtils.Сensorship;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;
using VeloBrawl.Tools.LaserData;

namespace VeloBrawl.Init;

public abstract class Initializer
{
    protected static void Start()
    {
        Saver.SearchTimeFactor = 1;
        Databases.AccountDatabase = new AccountDatabase("SaveBase/account_database.json");
        Databases.AllianceDatabase = new AllianceDatabase("SaveBase/alliance_database.json");
        Databases.NotificationIntraSignedDatabase =
            new NotificationIntraSignedDatabase("SaveBase/notification_inta_signed_database.json");
        Fingerprint.Patch = "SaveBase/Assets/fingerprint.json";
        LogicDataTables.CreateReferences("SaveBase/Assets/csv_logic/");
        ProfanityManager.Initialize("SaveBase/profanity.txt");
        GlobalStaticCloud.PragmaAndServerEnvironment = "dev";
        DynamicServerParameters.PathToJson = "SaveBase/event_structure.json";
        DynamicServerParameters.DefaultSecTimeForUpdate = 30;
        DynamicServerParameters.InitializeRotateEvents();
        DynamicServerParameters.EventRotator();
        DynamicServerParameters.ShopRotator();

        var ownMatchmakingManager = new OwnMatchmakingManager();
        {
            ownMatchmakingManager.Initialize();
        }

        Saver.OwnMatchmakingManager = ownMatchmakingManager;

        InitMaps();
    }

    [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH",
        MessageId = "type: VeloBrawl.Tools.LaserCsv.Data.LogicData[]; size: 231MB")]
    private static void InitMaps()
    {
        var obj = File.ReadAllText("SaveBase/map_structure.json");
        var mapDataList = JArray.Parse(obj);

        foreach (var jToken in mapDataList)
        {
            var mapData = (JObject)jToken;
            var name = "";
            {
                foreach (var data in LogicDataTables.GetAllDataFromCsvById(CsvHelperTable.Locations.GetId()))
                {
                    var d1 = (LogicLocationData)data;
                    if (mapData.GetValue("Map")!.ToString() == d1.GetMap()) name = d1.GetLocationTheme();
                }
            }

            var a1 = LogicDataTables.GetLocationThemeByName(name);
            Saver.MapStructureDictionary.Add(mapData.GetValue("Map")!.ToString(),
                [a1.GetMapHeight(), a1.GetMapWidth(), mapData.GetValue("Data")!.ToString()]);
        }
    }
}