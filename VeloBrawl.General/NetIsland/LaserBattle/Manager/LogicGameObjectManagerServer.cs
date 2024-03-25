﻿using VeloBrawl.General.NetIsland.LaserBattle.Objects;
using VeloBrawl.General.NetIsland.LaserBattle.Objects.Laser;
using VeloBrawl.General.NetIsland.LaserBattle.Render.Map;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.DataStream.Helps;
using VeloBrawl.Titan.Mathematical;
using VeloBrawl.Titan.Mathematical.Data;
using VeloBrawl.Titan.Utilities;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.General.NetIsland.LaserBattle.Manager;

public class LogicGameObjectManagerServer
{
    private readonly Queue<LogicGameObjectServer> _addObjects = new();
    private readonly List<LogicGameObjectServer> _gameObjects = [];
    private readonly Queue<LogicGameObjectServer> _removeObjects = new();
    private LogicBattleModeServer _logicBattleModeServer = null!;
    private int _objectCounter;

    public void Encode(BitStream bitStream, int ownObjectId, int playerIndex, int teamIndex)
    {
        var index = ObjectIndexHelperTableExtensions.GetOwnIndex(teamIndex, playerIndex);
        var tileMap = _logicBattleModeServer.GetTileMap();
        var gameModeVariation = LogicDataTables.GetGameModeVariationByName(
            ((LogicLocationData)LogicDataTables.GetDataById(_logicBattleModeServer.GetLocation() < 1000000
                ? GlobalId.CreateGlobalId(CsvHelperTable.Locations.GetId(), _logicBattleModeServer.GetLocation())
                : _logicBattleModeServer.GetLocation())).GetGameModeVariation()).GetVariation();
        var v1 = LogicMath.Clamp(tileMap.RenderSystem.GetTilemapWidth(), 0, tileMap.RenderSystem.GetTilemapWidth() - 1);
        var v2 = LogicMath.Clamp(tileMap.RenderSystem.GetTilemapHeight(), 0,
            tileMap.RenderSystem.GetTilemapHeight() - 1);
        var v3 = v1 >= 22 - 1;
        var v5 = _logicBattleModeServer.IsPlayFieldMirroredForPlayer(teamIndex);
        var v7 = -1;
        var v11 = _gameObjects.Where(obj =>
            obj != null! &&
            (obj.GetType() == ObjectTypeHelperTable.Projectile.GetObjectType() || obj.GetFadeCounterClient() > 0 ||
             obj.ShowedInInvisibleForTeams[teamIndex] || obj.GetIndex() / 16 == teamIndex) &&
            obj.GetObjectGlobalId() > 0).ToList();
        var v12 = _logicBattleModeServer.LogicPlayers.Where(obj => obj.OwnObjectId > 0).ToList();

        bitStream.WritePositiveIntMax2097151(ownObjectId);

        switch (gameModeVariation)
        {
            case 0:
                bitStream.WritePositiveVIntMax65535(0);
                break;
            case 100:
                bitStream.WritePositiveIntMax7(0);
                break;
            case 101:
                bitStream.WritePositiveIntMax7(1);
                bitStream.WritePositiveIntMax127(0);
                bitStream.WritePositiveIntMax127(0);
                break;
        }

        bitStream.WriteBoolean(v5);
        bitStream.WriteIntMax15(v7);

        bitStream.WriteBoolean(false);
        bitStream.WriteBoolean(false);
        bitStream.WriteBoolean(false);
        bitStream.WriteBoolean(false);

        if (v3)
        {
            bitStream.WritePositiveIntMax63(0);
            bitStream.WritePositiveIntMax63(0);
            bitStream.WritePositiveIntMax63(v1);
        }
        else
        {
            bitStream.WritePositiveIntMax31(0);
            bitStream.WritePositiveIntMax63(0);
            bitStream.WritePositiveIntMax31(v1);
        }

        bitStream.WritePositiveIntMax63(v2);

        for (var i = 0; i < tileMap.RenderSystem.GetTilemapWidth(); i++)
        for (var j = 0; j < tileMap.RenderSystem.GetTilemapHeight(); j++)
        {
            var tile = tileMap.LogicTileMap.GetTile(i, j, true);
            {
                if (tile.TileData.GetIsDestructible())
                    bitStream.WriteBoolean(tile.IsDestroyed());
            }
        }

        var v101 = bitStream.WritePositiveVIntMax65535OftenZero(tileMap.LogicTileMap.GetOriginalWallCount());
        {
            for (var i = 0; i < v101; i++)
            {
                var v101L = tileMap.LogicTileMap.GetTile(0, 0, true, true, _logicBattleModeServer, i);

                bitStream.WritePositiveIntMax4095(LogicMapLoader.GetTileIndex(_logicBattleModeServer, v101L.LogicX,
                    v101L.LogicY, false));
                bitStream.WritePositiveIntMax7(v101L.TileCode);
            }
        }

        var v102 = bitStream.WritePositiveVIntMax65535OftenZero(_logicBattleModeServer.LogicPetrolServers.Count);
        {
            for (var i = 0; i < v102; i++)
            {
                bitStream.WritePositiveIntMax63(_logicBattleModeServer.LogicPetrolServers[i].PetrolTileX);
                bitStream.WritePositiveIntMax63(_logicBattleModeServer.LogicPetrolServers[i].PetrolTileY);
                bitStream.WriteIntMax15(_logicBattleModeServer.LogicPetrolServers[i].PetrolType);
            }
        }

        for (var i = 0; i < v12.Count; i++)
        {
            var logicCharacterServer = (LogicCharacterServer)GetGameObjectById(v12[i].OwnObjectId);

            bitStream.WriteBoolean(v12[i].OwnObjectId > 0);
            bitStream.WriteBoolean((!logicCharacterServer.UltiEnabled(true) || i == index) && v12[i].HasUlti());

            if (gameModeVariation == 6) bitStream.WritePositiveIntMax15(0);
            logicCharacterServer.LogicAccessory?.Encode(bitStream, i == index);
            if (gameModeVariation == 3) bitStream.WriteBoolean(false);

            bitStream.WritePositiveVIntMax255OftenZero(0);

            if (v12[i].PinInfo.Count > 0)
                bitStream.WriteBoolean(_logicBattleModeServer.GetTicksGone() < LogicBattleModeServer.IntroTicks ||
                                       v12[i].PinInfo[1] > _logicBattleModeServer.GetTicksGone());
            else
                bitStream.WriteBoolean(_logicBattleModeServer.GetTicksGone() < LogicBattleModeServer.IntroTicks);
            {
                if (bitStream.WriteBoolean(_logicBattleModeServer.GetTicksGone() > LogicBattleModeServer.IntroTicks &&
                                           v12[i].PinInfo.Count > 0))
                {
                    bitStream.WriteIntMax7(v12[i].PinInfo[0]);
                    bitStream.WritePositiveIntMax16383(v12[i].PinInfo[2]);
                }
            }

            if (i != index) continue;
            bitStream.WritePositiveIntMax4095(v12[i].ChargeUlti(true, 0, 0));
            bitStream.WriteBoolean(v12[i].HasUlti());
            bitStream.WriteBoolean(false);
        }

        bitStream.WritePositiveVIntMax255OftenZero(0);

        if (LogicGameModeUtil.HasTimerAndCanEndBeforeTimerRunsOut(gameModeVariation))
            bitStream.WritePositiveVIntMax65535OftenZero(0);

        if (LogicGameModeUtil.RoundResetsWhenObjectiveIsMissing(gameModeVariation))
        {
            bitStream.WriteBoolean(true);
            bitStream.WriteIntMax1(0);
        }

        switch (gameModeVariation)
        {
            case 6:
                bitStream.WritePositiveIntMax15(0);
                break;
            case 16:
                bitStream.WritePositiveIntMax8191(0);
                bitStream.WritePositiveIntMax8191(0);
                break;
            case 9:
                bitStream.WritePositiveIntMax7(0);
                break;
            default:
                if (gameModeVariation == 5 || gameModeVariation == 8 ||
                    LogicGameModeUtil.HasTwoBases(gameModeVariation))
                {
                    bitStream.WritePositiveIntMax127(0);

                    if (LogicGameModeUtil.HasTwoBases(gameModeVariation))
                    {
                        bitStream.WritePositiveIntMax127(0);

                        if (gameModeVariation == 11)
                        {
                            bitStream.WritePositiveIntMax255(0);
                            bitStream.WritePositiveIntMax255(0);
                            bitStream.WritePositiveIntMax7(0);
                            bitStream.WritePositiveIntMax7(0);
                            bitStream.WritePositiveIntMax63(0);
                            bitStream.WritePositiveIntMax63(0);
                            bitStream.WriteBoolean(false);
                        }
                    }
                    else
                    {
                        bitStream.WritePositiveIntMax127(0);
                        bitStream.WriteBoolean(true);
                    }
                }
                else
                {
                    switch (gameModeVariation)
                    {
                        case 19:
                            bitStream.WritePositiveIntMax127(0);
                            bitStream.WritePositiveIntMax127(0);
                            break;
                        case 18:
                            bitStream.WritePositiveIntMax127(0);
                            break;
                        case 14:
                            bitStream.WritePositiveIntMax127(0);
                            bitStream.WritePositiveIntMax16383(0);
                            break;
                        case 13:
                            bitStream.WritePositiveIntMax131071(0);
                            break;
                        case 10:
                            bitStream.WritePositiveIntMax127(0);
                            break;
                        case 7:
                            bitStream.WritePositiveIntMax127(0);
                            break;
                    }
                }

                break;
        }

        for (var i = 0; i < v12.Count; i++)
        {
            var v200 = true;
            {
                if (gameModeVariation == 14)
                    if (v200)
                        if (0 < 1) // if (player damage < 1)
                            v200 = false;
            }

            if (bitStream.WriteBoolean(v200))
                switch (gameModeVariation)
                {
                    case 14:
                        bitStream.WritePositiveIntMax524287(0);
                        break;
                    case 15:
                        bitStream.WritePositiveIntMax134217727(0);
                        break;
                    default:
                        bitStream.WritePositiveVIntMax255(0);
                        break;
                }

            if (!bitStream.WriteBoolean(false)) continue;
            {
                bitStream.WritePositiveIntMax15(0);
                for (var j = 0; j < 0; j++)
                {
                    bitStream.WritePositiveIntMax15(0);
                    bitStream.WriteIntMax7(0);
                }
            }
        }

        var v103 = bitStream.WritePositiveVIntMax65535(v11.Count);
        {
            if (v103 > 0)
            {
                foreach (var gameObject in v11)
                    ByteStreamHelper.WriteDataReference(bitStream, gameObject.GetData());
                foreach (var gameObject in v11)
                    ByteStreamHelper.WriteObjectRunningId(bitStream, gameObject.GetObjectGlobalId());
                foreach (var gameObject in v11)
                    gameObject.Encode(bitStream, ownObjectId == gameObject.GetObjectGlobalId(), ownObjectId, teamIndex);
            }
        }

        if (gameModeVariation != 7) return;
        if (!bitStream.WriteBoolean(false)) return;

        bitStream.WritePositiveIntMax32767(0);
        bitStream.WritePositiveIntMax65535(0);
    }

    public LogicBattleModeServer GetLogicBattleModeServer()
    {
        return _logicBattleModeServer;
    }

    public void SetLogicBattleModeServer(LogicBattleModeServer logicBattleModeServer)
    {
        _logicBattleModeServer = logicBattleModeServer;
    }

    public LogicGameObjectServer[] GetGameObjects(bool copy = false)
    {
        if (!copy) return _gameObjects.ToArray();

        var copyArray = new LogicGameObjectServer[_gameObjects.Count];
        _gameObjects.CopyTo(copyArray);

        return copyArray;
    }

    public void AddLogicGameObject(LogicGameObjectServer logicGameObjectServer)
    {
        logicGameObjectServer.AttachLogicGameObjectManager(this,
            GlobalId.CreateGlobalId(logicGameObjectServer.GetType(), _objectCounter++));
        {
            _addObjects.Enqueue(logicGameObjectServer);
        }
    }

    public void RemoveGameObjectReferences(LogicGameObjectServer logicGameObjectServer)
    {
        _removeObjects.Enqueue(logicGameObjectServer);
    }

    public LogicGameObjectServer GetGameObjectById(int globalId)
    {
        return _gameObjects.Find(obj => obj.GetObjectGlobalId() == globalId)!;
    }

    public List<LogicGameObjectServer> GetNumGameObjects()
    {
        return _gameObjects;
    }

    public void PreTick()
    {
        var destructibleObjects = _gameObjects.Where(gameObject => gameObject != null!)
            .Where(gameObject => gameObject.ShouldDestruct()).ToList();

        Parallel.ForEach(destructibleObjects, gameObject =>
        {
            gameObject.Destruct();
            RemoveGameObjectReferences(gameObject);
        });

        while (_addObjects.Count > 0) _gameObjects.Add(_addObjects.Dequeue());

        while (_removeObjects.Count > 0) _ = _gameObjects.Remove(_removeObjects.Dequeue());
    }

    public void Tick()
    {
        try
        {
            Parallel.ForEach(_gameObjects, gameObject => gameObject.Tick());
        }
        catch
        {
            // ignored.
        }
    }
}