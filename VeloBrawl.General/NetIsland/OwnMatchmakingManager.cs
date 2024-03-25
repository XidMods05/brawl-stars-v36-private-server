using VeloBrawl.Logic.Dynamic;
using VeloBrawl.Titan.Utilities;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.General.NetIsland;

public class OwnMatchmakingManager
{
    public Dictionary<int, MatchmakingManager> MatchmakingManagers { get; } = new();

    public void Initialize()
    {
        foreach (var variable in DynamicServerParameters.Event1DataMassive)
        {
            var mm = new MatchmakingManager();
            {
                mm.Initialize(variable.Value.GetSlot(),
                    LogicGamePlayUtil.GetPlayerCountWithGameModeVariation(
                        LogicDataTables
                            .GetGameModeVariationByName(
                                ((LogicLocationData)LogicDataTables.GetDataById(variable.Value.GetLocation()))
                                .GetGameModeVariation()).GetVariation()));
            }

            MatchmakingManagers.Add(variable.Value.GetSlot(), mm);
        }
    }
}