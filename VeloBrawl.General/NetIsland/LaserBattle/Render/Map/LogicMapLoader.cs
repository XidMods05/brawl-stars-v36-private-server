﻿using VeloBrawl.General.NetIsland.LaserBattle.Render.Tile;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.General.NetIsland.LaserBattle.Render.Map;

public static class LogicMapLoader
{
    public static LogicTileData TileCodeToTileData(char code)
    {
        foreach (var data in LogicDataTables.GetAllDataFromCsvById(CsvHelperTable.Tiles.GetId()))
        {
            if (data is not LogicTileData tileData) continue;
            if (tileData.GetTileCode() != code) continue;

            return tileData;
        }

        return (LogicTileData)LogicDataTables.GetAllDataFromCsvById(CsvHelperTable.Tiles.GetId())[0];
    }

    public static int GetTileIndex(LogicBattleModeServer logicBattleModeServer, int x, int y, bool isTileCords)
    {
        if (!isTileCords)
            return (LogicTileMap.LogicToTile(y) + 1) *
                logicBattleModeServer.GetTileMap().RenderSystem.GetTilemapWidth() -
                logicBattleModeServer.GetTileMap().RenderSystem.GetTilemapWidth() + LogicTileMap.LogicToTile(x);

        x = LogicTileMap.TileToLogic(x);
        y = LogicTileMap.TileToLogic(y);

        return (LogicTileMap.LogicToTile(y) + 1) *
            logicBattleModeServer.GetTileMap().RenderSystem.GetTilemapWidth() -
            logicBattleModeServer.GetTileMap().RenderSystem.GetTilemapWidth() + LogicTileMap.LogicToTile(x);
    }
}