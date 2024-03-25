using VeloBrawl.Titan.Mathematical.Data;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Data;

namespace VeloBrawl.General.NetIsland.LaserBattle.Factory;

public static class LogicGameObjectFactoryServer
{
    public static int CreateGameObjectByClassId(int classId)
    {
        return CreateGameObjectByData(LogicDataTables.GetDataById(GlobalId.CreateGlobalId(classId, 0)));
    }

    public static int CreateGameObjectByData(LogicData logicData)
    {
        return logicData.GetDataType();
    }
}