﻿using VeloBrawl.General.Cloud;
using VeloBrawl.General.NetIsland.LaserBattle.Render.Rect;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.Mathematical.Data;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.General.NetIsland.LaserBattle.Render.Tile.Metadata;

public class LogicTileMetadata
{
    public LogicTileMetadata(int locationId)
    {
        var u1 = ((LogicLocationData)LogicDataTables.GetDataById(locationId < 1000000
            ? GlobalId.CreateGlobalId(CsvHelperTable.Locations.GetId(), locationId)
            : locationId)).GetMap();
        var u2 = Saver.MapStructureDictionary[u1];

        var v1 = Convert.ToInt32(u2[0]); // height.
        var v2 = Convert.ToInt32(u2[1]); // width.
        var v3 = u2[2].ToString(); // data.

        var renderSystem = new RenderSystem();
        {
            renderSystem.Height = v1;
            renderSystem.Width = v2;
        }

        var logicTileMap = new LogicTileMap(renderSystem);
        {
            renderSystem.LogicTileMap = logicTileMap;
        }

        var logicRect = new LogicRect(0, 0, LogicTileMap.TileToLogic(renderSystem.GetTilemapWidth()),
            LogicTileMap.TileToLogic(renderSystem.GetTilemapHeight()));

        LogicGlobalStringData = v3!;
        RenderSystem = renderSystem;
        LogicTileMap = logicTileMap;
        LogicRect = logicRect;
    }

    public string LogicGlobalStringData { get; private set; }
    public RenderSystem RenderSystem { get; private set; }
    public LogicTileMap LogicTileMap { get; private set; }
    public LogicRect LogicRect { get; private set; }
}