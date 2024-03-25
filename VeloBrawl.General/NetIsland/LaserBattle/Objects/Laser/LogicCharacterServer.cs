using System.Reflection;
using VeloBrawl.General.NetIsland.LaserBattle.Render.Help;
using VeloBrawl.General.NetIsland.LaserBattle.Significant;
using VeloBrawl.General.NetIsland.LaserBattle.Significant.Accessory;
using VeloBrawl.General.NetIsland.LaserBattle.Significant.Fly;
using VeloBrawl.Logic.Environment.LaserMessage.Laser.Node_1;
using VeloBrawl.Supercell.Titan.CommonUtils.Utils;
using VeloBrawl.Titan.DataStream;
using VeloBrawl.Titan.Mathematical;
using VeloBrawl.Titan.Mathematical.Data;
using VeloBrawl.Titan.Utilities;
using VeloBrawl.Tools.LaserCsv;
using VeloBrawl.Tools.LaserCsv.Manufacturer.Laser;

namespace VeloBrawl.General.NetIsland.LaserBattle.Objects.Laser;

public class LogicCharacterServer(LogicBattleModeServer logicBattleModeServer, int classId, int instanceId, int index)
    : LogicGameObjectServer(logicBattleModeServer, classId, instanceId, 0, index)
{
    public const int DamageNumbersDelayTicks = 2;

    private readonly int _classId = classId;
    private readonly int _instanceId = instanceId;
    private readonly LogicBattleModeServer _logicBattleModeServer1 = logicBattleModeServer;
    private int _afkTicks;

    private int _attackAngle;
    private int _attackingTicker;
    private int _characterState;
    private FlyParticle _flyParticle = null!;

    private int _gameModeVariation;
    private int _interruptSkillsTick;
    private bool _isMoving;
    private bool _isPlayerControlRemoved;
    private float _localSpeedFactor;

    private LogicCharacterData? _logicCharacterData;
    private LogicSkillData _logicSkillData1 = null!;
    private LogicSkillData _logicSkillData2 = null!;
    private List<LogicSkillServer> _logicSkillServers = [];
    private int _maxHitPoints;
    private int _moveAngle;
    private int _movementCount;
    private int _moveTicker;
    private int _nowHitPoints;
    private LogicVector2 _nowPosition = new();
    private int _oldDa;
    private int _oldDx;
    private int _oldDy;
    private LogicVector2 _requiredPosition = new();
    private LogicVector2 _requiredPositionDelta = new();
    private LogicVector2 _startPosition = new();
    private bool _ultimateVisualEffect;

    public LogicAccessory? LogicAccessory;

    public void InitializeMembers(bool playerSector = false)
    {
        if (!playerSector)
        {
            _gameModeVariation = LogicDataTables.GetGameModeVariationByName(
                ((LogicLocationData)LogicDataTables.GetDataById(_logicBattleModeServer1.GetLocation() < 1000000
                    ? GlobalId.CreateGlobalId(CsvHelperTable.Locations.GetId(), _logicBattleModeServer1.GetLocation())
                    : _logicBattleModeServer1.GetLocation())).GetGameModeVariation()).GetVariation();

            _logicCharacterData =
                (LogicCharacterData)LogicDataTables.GetDataById(GlobalId.CreateGlobalId(_classId, _instanceId));

            if (_logicCharacterData.GetWeaponSkill() != "")
                _logicSkillData1 = LogicDataTables.GetSkillByName(_logicCharacterData.GetWeaponSkill());
            if (_logicCharacterData.GetUltimateSkill() != "")
                _logicSkillData2 = LogicDataTables.GetSkillByName(_logicCharacterData.GetUltimateSkill());

            _logicSkillServers = [];
            {
                if (_logicCharacterData.GetUltimateSkill() != "")
                {
                    _logicSkillServers.Add(new LogicSkillServer(GetLogicBattleModeServer(),
                        _logicSkillData1.GetClassId(),
                        _logicSkillData1.GetInstanceId()));
                    _logicSkillServers.Add(new LogicSkillServer(GetLogicBattleModeServer(),
                        _logicSkillData2.GetClassId(),
                        _logicSkillData2.GetInstanceId()));
                }
            }

            _maxHitPoints = _logicCharacterData.GetHitpoints();
            _nowHitPoints = _maxHitPoints;
            _interruptSkillsTick = 0;
            _movementCount = 1;
            _moveTicker = -1;
            _attackingTicker = 63;
            _attackAngle = 0;
            _afkTicks = 0;
            _characterState = 0;
            _oldDa = 0;
            _oldDx = 0;
            _oldDy = 0;
            _localSpeedFactor = 1;
            _isMoving = false;
            _isPlayerControlRemoved = false;
            _ultimateVisualEffect = false;
        }

        // player sector.
        if (!_logicCharacterData!.IsHero() || !playerSector) return;

        if (GetPlayer()!.CardInfo.Count > 1)
            LogicAccessory = new LogicAccessory(this,
                LogicDataTables.GetAccessoryByName(GetPlayer()!.CardInfo[1].GetSkill()));
        if (GetPlayer()!.GetTeamIndex() == 0) _moveAngle = 90;
        else _moveAngle = 90 * 3;

        GetPlayer()!.ChargeUlti(false, 10000, 100);
    }

    public override void Tick()
    {
        if (_logicBattleModeServer1.GetTicksGone() <= LogicBattleModeServer.IntroTicks) return;

        TickSelfDestruct();
        TickTimers();
        HandleMoveAndAttack();

        if (_flyParticle != null!)
        {
            if (!_flyParticle.Update())
                _flyParticle = null!;
            else _afkTicks = -1;
        }

        LogicAccessory?.UpdateAccessory();
        
        foreach (var logicSkillServer in _logicSkillServers)
        {
            logicSkillServer.Tick();
        }
    }

    private void TickSelfDestruct()
    {
        var fields = ((object)this).GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        {
            foreach (var field in fields)
            {
                if (!field.Name.StartsWith('_') || !field.Name.EndsWith("Tick")) continue;

                var value = (int)field.GetValue(this)!;
                {
                    if (value < _logicBattleModeServer1.GetTicksGone()) field.SetValue(this, -1);
                }
            }
        }
    }

    private void TickTimers()
    {
        if (!_logicCharacterData!.IsHero()) return; // player sector.

        _afkTicks++;
        if (_logicBattleModeServer1.IsPlayerAfk(_afkTicks, this))
        {
            var disconnectedMessage = new DisconnectedMessage();
            {
                disconnectedMessage.SetReason(2);
            }

            /*_logicBattleModeServer1.LogicPlayersConnection[GetPlayer()!].GetMessaging()!
                .Send(disconnectedMessage);*/
        }
    }

    public void SetStartPosition(LogicVector2 logicVector2)
    {
        _startPosition = logicVector2.Clone();
        {
            SetPosition(_startPosition.GetX(), _startPosition.GetY(), GetZ());
        }

        _nowPosition = _startPosition.Clone();
        _requiredPosition = _startPosition.Clone();
        _requiredPositionDelta = _startPosition.Clone();
    }

    public void MoveTo(int toX, int toY, int toXDelta, int toYDelta, bool flyMode = false)
    {
        if (!_logicBattleModeServer1.GetTileMap().LogicRect.IsInside(toX, toY)) return;
        if (_flyParticle != null!) return;

        _requiredPosition = new LogicVector2(toX, toY);

        if (flyMode)
        {
            _isMoving = false;
            _characterState = 0;
            _moveTicker = 0;

            var deltaTile = _logicBattleModeServer1.GetTileMap().LogicTileMap.GetTile(toX, toY);

            if (deltaTile.TileData.GetTileCode() == 'W')
            {
                var deltaTileDictionary = new Dictionary<int, LogicVector2>();

                for (var pseudoX = 0;
                     pseudoX < _logicBattleModeServer1.GetTileMap().RenderSystem.GetTilemapWidth();
                     pseudoX++)
                for (var pseudoY = 0;
                     pseudoY < _logicBattleModeServer1.GetTileMap().RenderSystem.GetTilemapHeight();
                     pseudoY++)
                {
                    var tile = _logicBattleModeServer1.GetTileMap().LogicTileMap
                        .GetTile(pseudoX, pseudoY, true);
                    if (tile.TileData.GetTileCode() != '.') continue;

                    var v1 = new LogicVector2(tile.LogicX, tile.LogicY);
                    var v2 = new LogicVector2(toX, toY);

                    deltaTileDictionary.TryAdd(v1.GetDistance(v2), v1);
                }

                var targetVector = deltaTileDictionary[deltaTileDictionary.Keys.Min()];
                {
                    _requiredPosition.Set(targetVector.GetX(), targetVector.GetY());
                }
            }

            var v2A = _requiredPosition.Clone();
            {
                v2A.Substract(GetPosition().Clone());
            }

            var deltaXproto = _requiredPosition.Clone().GetX() - GetPosition().Clone().GetX() > 0
                ? LogicMath.Min(2700,
                    _requiredPosition.Clone().GetX() - GetPosition().Clone().GetX())
                : LogicMath.Max(-2700,
                    _requiredPosition.Clone().GetX() - GetPosition().Clone().GetX());

            var deltaYproto = _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY() > 0
                ? LogicMath.Min(2700,
                    _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY())
                : LogicMath.Max(-2700,
                    _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY());

            _moveAngle = LogicMath.GetAngle(deltaXproto, deltaYproto);
            _flyParticle = new FlyParticle(_requiredPosition);
            _flyParticle.SetParent(this, 3000);
            return;
        }

        _moveTicker = GetLogicBattleModeServer().GetTicksGone() + 100 + _logicBattleModeServer1.GetTick() / 20;
        _oldDa = _moveAngle;

        if (_isMoving) return;

        _movementCount++;
        _isMoving = true;

        _nowPosition = _requiredPosition.Clone();
        _requiredPosition = new LogicVector2(toX, toY);
        _requiredPositionDelta = new LogicVector2(toXDelta, toYDelta);
    }

    public void ActivateSkill(int x, int y, int skillType)
    {
        var v2A = new LogicVector2(x, y);

        _attackingTicker = skillType;
        _characterState = 2;

        _attackAngle = v2A.GetAngle();
        _moveAngle = _attackAngle - 1;
        _ultimateVisualEffect = false;

        if (_logicSkillServers[skillType].GetData().GetBehaviorType().Equals("Charge"))
            foreach (var logicVector2 in FigureCreator.Heart.DrawFigure(v2A, 60))
            {
                var logicAreaEffectServer = new LogicAreaEffectServer(_logicBattleModeServer1,
                    CsvHelperTable.AreaEffects.GetId(),
                    184, GetIndex());
                {
                    logicAreaEffectServer.SetPosition(logicVector2);
                    logicAreaEffectServer.ChangeFadeCounterServer(6);
                }

                GetLogicGameObjectManager().AddLogicGameObject(logicAreaEffectServer);
            }
        
        _ = skillType > 0 ? _logicSkillServers[1].Activate(false, new LogicVector2(x, y)) : 
            _logicSkillServers[0].Activate(false, new LogicVector2(x, y));
        
        if (skillType > 0 && GetPlayer() != null) GetPlayer()!.ChargeUlti(false, -4000, 100);
    }

    public void HandleMoveAndAttack()
    {
        const bool moveSectorAvailable = true;
        {
            if (moveSectorAvailable)
            {
                var movingSpeed = GetMovementSpeed() *
                                  _logicBattleModeServer1.GetMovementSpeedFactor(0,
                                      GetX(), GetY(),
                                      _requiredPosition.Clone().GetX(), _requiredPosition.Clone().GetY());

                if (GetLogicBattleModeServer().GetTicksGone() < _moveTicker || _moveTicker < 0)
                    if (GetPosition().Clone().GetDistance(_requiredPosition.Clone()) != 0 && movingSpeed > 0 &&
                        _isMoving)
                    {
                        if (_requiredPosition.Clone().GetX() - GetPosition().Clone().GetX() != 0 ||
                            _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY() != 0)
                        {
                            var v2A = _requiredPosition.Clone();
                            {
                                v2A.Substract(GetPosition().Clone());
                            }

                            var deltaXproto = _requiredPosition.Clone().GetX() - GetPosition().Clone().GetX() > 0
                                ? LogicMath.Min(movingSpeed,
                                    _requiredPosition.Clone().GetX() - GetPosition().Clone().GetX())
                                : LogicMath.Max(-movingSpeed,
                                    _requiredPosition.Clone().GetX() - GetPosition().Clone().GetX());

                            var deltaYproto = _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY() > 0
                                ? LogicMath.Min(movingSpeed,
                                    _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY())
                                : LogicMath.Max(-movingSpeed,
                                    _requiredPosition.Clone().GetY() - GetPosition().Clone().GetY());

                            var deltaX = (int)(LogicMath.Cos(LogicMath.GetAngle(v2A.GetX(), v2A.GetY())) /
                                               (_logicBattleModeServer1.GetTick() * 1000 + 1000 * 2 -
                                                movingSpeed * _localSpeedFactor) *
                                               _logicCharacterData!.GetSpeed() * _localSpeedFactor);

                            var deltaY = (int)(LogicMath.Sin(LogicMath.GetAngle(v2A.GetX(), v2A.GetY())) /
                                               (_logicBattleModeServer1.GetTick() * 1000 + 1000 * 2 -
                                                movingSpeed * _localSpeedFactor) *
                                               _logicCharacterData.GetSpeed() * _localSpeedFactor);

                            var deltaTile = _logicBattleModeServer1.GetTileMap().LogicTileMap
                                .GetTile(deltaXproto + GetX(), deltaYproto + GetY());

                            if ((!deltaTile.IsDestroyed() && deltaTile.TileData.GetBlocksMovement()) ||
                                deltaTile.TileData.GetTileCode() == 'W')
                            {
                                var deltaTileDictionary = new Dictionary<int, LogicVector2>();

                                for (var pseudoX = 0;
                                     pseudoX < _logicBattleModeServer1.GetTileMap().RenderSystem.GetTilemapWidth();
                                     pseudoX++)
                                for (var pseudoY = 0;
                                     pseudoY < _logicBattleModeServer1.GetTileMap().RenderSystem.GetTilemapHeight();
                                     pseudoY++)
                                {
                                    var tile = _logicBattleModeServer1.GetTileMap().LogicTileMap
                                        .GetTile(pseudoX, pseudoY, true);
                                    if (tile.TileData.GetTileCode() != '.') continue;

                                    var v1 = new LogicVector2(tile.LogicX, tile.LogicY);
                                    var v2 = new LogicVector2(deltaXproto + GetX(), deltaYproto + GetY());

                                    deltaTileDictionary.TryAdd(v1.GetDistance(v2), v1);
                                }

                                var targetVector = deltaTileDictionary[deltaTileDictionary.Keys.Min()];
                                {
                                    GetPosition().Set(targetVector.GetX(), targetVector.GetY());

                                    _nowPosition = GetPosition().Clone();
                                    _isMoving = false;

                                    return;
                                }
                            }

                            var v1d = GetPosition().Clone();
                            {
                                v1d.Add(GetPosition().GetDistance(_requiredPosition.Clone()) > 360 / 10
                                    ? new LogicVector2(deltaX, deltaY)
                                    : new LogicVector2(deltaXproto, deltaYproto));
                            }
                            
                            GetPosition().Set(v1d.GetX() + 1, v1d.GetY());
                            
                            if (_attackingTicker > 60)
                            {
                                if (LogicMath.Abs(_moveAngle -
                                                  LogicMath.NormalizeAngle360(LogicMath.GetAngle(deltaX, deltaY))) > 1)
                                    if (LogicMath.Abs(_oldDa -
                                                      LogicMath.NormalizeAngle360(LogicMath.GetAngle(deltaX, deltaY))) >
                                        1)
                                        if ((LogicMath.Cos(deltaXproto) + LogicMath.Sin(deltaYproto)) / 360 <= 360)
                                        {
                                            _moveAngle =
                                                LogicMath.NormalizeAngle360(LogicMath.GetAngle(deltaX, deltaY));
                                            _attackAngle = _moveAngle;
                                            _oldDa = _attackAngle;
                                        }
                            }

                            _oldDx = deltaX;
                            _oldDy = deltaY;
                            _isMoving = false;
                        }

                        if (GetPosition().GetDistance(_requiredPosition.Clone()) <= 1 + 1)
                            GetPosition().Set(_requiredPosition);

                        _isMoving = GetPosition().GetDistance(_requiredPosition.Clone()) != 0;
                        {
                            _nowPosition = GetPosition().Clone();
                        }
                    }
            }
        }

        const bool attackSectorAvailable = true;
        {
            if (attackSectorAvailable)
            {
            }
        }

        const bool hybridSectorAvailable = true;
        {
            if (hybridSectorAvailable)
            {
                if (_attackingTicker < 63) _attackingTicker += 15;
                {
                    _attackingTicker = LogicMath.Clamp(_attackingTicker, 0, 63);
                }

                if (_attackingTicker >= 63) _characterState = _isMoving ? 1 : 0;
            }
        }
    }

    public LogicVector2 GetShootPositionModifiers(int a1, int a2, int a3, int a4, bool a5, int a6, int a7)
    {
        int v24, v25;
        var v9 = a3 - a1;
        var v11 = a4 - a2;
        var v12 = LogicMath.Sqrt(v9 * v9 + v11 * v11);
        var v13 = v9;
        var v14 = v11;

        if (v12 != 0)
        {
            v13 = v9 * a6 / v12;
            v14 = v11 * a6 / v12;
        }

        var v16 = a5;
        var v17 = LogicMath.GetRotatedX(v9, v11, 90);
        var v18 = LogicMath.GetRotatedY(v9, v11, 90);

        var v19 = LogicMath.Sqrt(v17 * v17 + v18 * v18);
        {
            if (v19 != 0)
            {
                v17 = v17 * a7 / v19;
                v16 = a5;
                v18 = v18 * a7 / v19;
            }
        }

        var v21 = v13 + a1;
        var v23 = v14 + a2;

        if (v16)
        {
            v24 = v23 - v18;
            v25 = v21 - v17;
        }
        else
        {
            v24 = v18 + v23;
            v25 = v17 + v21;
        }

        return new LogicVector2(v25, v24);
    } // LogicVector2 ShootPosition = GetShootPositionModifiers(GetX(), GetY(), GetX() + x, GetY() + y, Index % 2 == 0, 50, CharacterData.TwoWeaponAttackEffectOffset);

    public bool UltiEnabled(bool isCheckMode = false)
    {
        if (isCheckMode) return _ultimateVisualEffect;
        return _ultimateVisualEffect = true;
    }

    public void UltiDisabled()
    {
        _ultimateVisualEffect = false;
    }

    public void ResetAfkTicks()
    {
        _afkTicks = 0;
    }

    public void InterruptAllSkills()
    {
        _interruptSkillsTick = _logicBattleModeServer1.GetTicksGone() +
                               _logicBattleModeServer1.GetTick() / 2;
    }

    public bool IsPlayerControlRemoved()
    {
        return _isPlayerControlRemoved;
    }

    public override void Encode(BitStream bitStream, bool isOwnObject, int visionIndex, int visionTeam)
    {
        base.Encode(bitStream, isOwnObject, visionIndex, visionTeam);

        var v0 = isOwnObject && _logicCharacterData!.IsHero();
        var v1 = IsPlayerControlRemoved();
        var v2 = LogicMath.Clamp(-1, 0, 255);
        var v3 = _logicCharacterData!.IsBoss();
        var v4 = (_logicCharacterData!.HasVeryMuchHitPoints() && _logicCharacterData.IsHero()) ||
                 _logicCharacterData.GetHitpoints() > 65535;
        var v5 = false; // isBigGameBoss;
        var v6 = false; // getChargedShotCount >= 1;
        var v7 = _localSpeedFactor > 1;
        var v8 = LogicMath.Clamp((int)_localSpeedFactor * 100, 1, 1023);
        var v9 = LogicMath.Clamp(_attackingTicker, 0, 63);
        var v10 = false;

        if (_logicCharacterData.IsHero()) v6 = _logicSkillData1.GetChargedShotCount() >= 1;

        if (_logicCharacterData.GetSpeed() > 0 || _logicCharacterData.HasAutoAttack() ||
            _logicCharacterData.IsTrainingDummy())
        {
            switch (v0)
            {
                case true:
                {
                    bitStream.WriteBoolean(v1); // isPlayerControlRemoved;
                    bitStream.WriteBoolean(false); // only false;

                    if (v1)
                    {
                        bitStream.WritePositiveIntMax511(_moveAngle); // moveAngle;
                        bitStream.WritePositiveIntMax511(_attackAngle); // attackAngle;
                    }

                    break;
                }

                case false:
                {
                    bitStream.WritePositiveIntMax511(_moveAngle); // moveAngle;
                    bitStream.WritePositiveIntMax511(_attackAngle); // attackAngle;
                    break;
                }
            }

            bitStream.WritePositiveIntMax7(_characterState); // character state;
            {
                if (_logicCharacterData.GetHitpoints() > 3)
                {
                    bitStream.WriteBoolean(false); // mini buff;
                    bitStream.WriteIntMax63(v9); // attacking ticks;
                    bitStream.WriteBoolean(false); // angle mirroring;
                    if (bitStream.WriteBoolean(false)) bitStream.WriteBoolean(false); // if (stunned);
                    bitStream.WriteBoolean(LogicAccessory != null); // accessory visualization (gadget) [in v36 not work];
                    bitStream.WriteBoolean(false); // accessory visualization (star power);
                }
            }
        }
        else
        {
            bitStream.WritePositiveIntMax7(_characterState); // character state;

            if (_logicCharacterData.IsTrain())
            {
                bitStream.WritePositiveIntMax511(_moveAngle); // moveAngle;
                bitStream.WritePositiveIntMax511(_attackAngle); // attackAngle;
            }
            else if (_logicCharacterData.GetAreaEffect() != "")
            {
                bitStream.WritePositiveIntMax511(_moveAngle); // moveAngle;
            }
        }

        bitStream.WritePositiveVIntMax65535OftenZero(0); // all part effect tick;
        bitStream.WritePositiveVIntMax65535OftenZero(0); // bibi attack effect tick;

        bitStream.WriteBoolean(false); // mega speed buff without movement animation;
        bitStream.WriteBoolean(v7); // micro speed buff with movement animation;
        bitStream.WriteBoolean(false); // slow mode;
        bitStream.WriteBoolean(false); // red animation before slow mode;
        bitStream.WriteBoolean(false); // only false;
        bitStream.WriteBoolean(false); // only false;
        bitStream.WriteBoolean(false); // belle ultimate effect;
        bitStream.WriteBoolean(false); // only false (otis prototype);

        bitStream.WritePositiveVIntMax255OftenZero(0); // lou freeze percentage;

        var a101 = bitStream.WritePositiveVIntMax255OftenZero(0); // LogicPoisonServer count;
        {
            for (var i = 0; i < a101; i++)
            {
                var a101L = bitStream.WritePositiveIntMax7(0); // poison type;
                bitStream.WriteBoolean(a101L is 0 or 4); // poison unknown parameter;
            }
        }

        bitStream.WritePositiveVIntMax255OftenZero(v2); // heal animation tick;
        {
            if (v3)
            {
                bitStream.WritePositiveIntMax2097151(_nowHitPoints); // now hp;
                bitStream.WritePositiveIntMax2097151(_maxHitPoints); // max hp;
            }
            else
            {
                if (v4 && !_logicCharacterData.IsHero())
                {
                    bitStream.WritePositiveIntMax524287(_nowHitPoints); // now hp;
                    bitStream.WritePositiveIntMax524287(_maxHitPoints); // max hp;
                }
                else
                {
                    if (v5)
                    {
                        bitStream.WritePositiveIntMax262143(_nowHitPoints); // now hp;
                        bitStream.WritePositiveIntMax262143(_maxHitPoints); // max hp;
                    }
                    else
                    {
                        bitStream.WritePositiveVIntMax65535(_nowHitPoints); // now hp;
                        bitStream.WritePositiveVIntMax65535(_maxHitPoints); // max hp;
                    }
                }
            }
        }

        if (_logicCharacterData.IsDecoy()) bitStream.WriteBoolean(false); // clone have ultimate;

        if (_logicCharacterData.IsHero() || _logicCharacterData.IsDecoy())
        {
            bitStream.WritePositiveVIntMax255OftenZero(0); // default score + this field value;

            if (bitStream.WriteBoolean(false)) // custom shield;
            {
                bitStream.WritePositiveIntMax2047(0); // percentage now;
                bitStream.WritePositiveIntMax2047(0); // percentage start;
            }

            bitStream.WriteBoolean(false); // purple rage;
        }

        if (_logicCharacterData.IsHero())
        {
            if (LogicGameModeUtil.ModeHasCarryables(_gameModeVariation)) bitStream.WriteBoolean(false);

            bitStream.WritePositiveVIntMax255OftenZero(0); // only 0;
            bitStream.WriteBoolean(false); // big hero;
            bitStream.WriteBoolean(false); // poison and down effect;

            if (bitStream.WriteBoolean(true)) // only true;
            {
                bitStream.WriteBoolean(false); // bugging visor;
                bitStream.WriteBoolean(false); // white shield;
                bitStream.WriteBoolean(false); // 8-bit teleport;
                bitStream.WriteBoolean(false); // 8-bit teleport x2;
                bitStream.WriteBoolean(_ultimateVisualEffect); // ultimate visualization;
            }

            bitStream.WriteBoolean(false); // unknown;
            
            if (v0)
                if (bitStream.WriteBoolean(false)) // static visor (volleyball map effect);
                {
                    bitStream.WriteIntMax65535(0); // x-visor;
                    bitStream.WriteIntMax65535(0); // y-visor;
                }

            if (v6) bitStream.WriteIntMax3(0); // gigSting type;

            if (bitStream.WriteBoolean(false))
            {
                bitStream.WritePositiveIntMax15(10); // unknown;
                bitStream.WritePositiveIntMax7(0); // short effect with sound;
            }

            if (v10) // two part yellow shield;
                if (bitStream.WriteBoolean(true))
                {
                    bitStream.WritePositiveVIntMax65535(0);
                    bitStream.WritePositiveVIntMax65535(0);
                }
        }

        if (!_logicCharacterData.IsCarryable())
        {
            switch (_logicCharacterData.GetName())
            {
                case "RopeDude":
                    bitStream.WriteBoolean(false); // zone fixer;
                    break;
                case "PowerLeveler":
                    bitStream.WritePositiveIntMax3(0); // level fixer;
                    break;
                case "Baseball":
                    bitStream.WritePositiveIntMax31(0); // home-run fixer;
                    break;
                case "FireDude":
                    bitStream.WritePositiveIntMax31(0); // petrol fixer;
                    break;
            }
        }
        else
        {
            bitStream.WritePositiveIntMax7(0); // carryable effect;
            bitStream.WritePositiveIntMax3(0); // unknown;
        }

        if (v3) bitStream.WriteBoolean(false); // unknown boss parameter;

        bitStream.WriteBoolean(false); // yellow shield;
        bitStream.WritePositiveIntMax3(0); // global effect;
        bitStream.WriteBoolean(false); // not fully visible;
        bitStream.WritePositiveIntMax511(0); // only 0;

        if (v0)
        {
            if (bitStream.WriteBoolean(v7)) bitStream.WriteIntMax1023(v8); // speed buff percentage;
            bitStream.WriteBoolean(false); // tara's eye;
        }

        bitStream.WritePositiveIntMax31(0); // damage array;

        foreach (var logicSkillServer in _logicSkillServers)
            logicSkillServer.Encode(bitStream, _interruptSkillsTick > 0);
    }

    public override int GetType()
    {
        return ObjectTypeHelperTable.Character.GetObjectType();
    }

    public override int GetRadius()
    {
        return _logicCharacterData!.GetCollisionRadius();
    }

    public int GetMovementSpeed()
    {
        return Convert.ToInt32((_logicCharacterData!.GetSpeed() / (float)_logicBattleModeServer1.GetTick() -
                                _logicBattleModeServer1.GetTick()) * _localSpeedFactor);
    }

    public float GetSpeedBuff()
    {
        return _localSpeedFactor;
    }

    public LogicCharacterData GetCharacterData()
    {
        return _logicCharacterData!;
    }
}