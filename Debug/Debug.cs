namespace T4.Plugins.Troubadour;

public sealed class Debug : TroubadourPlugin, IGameWorldPainter, IGameUserInterfacePainter, IRenderEnabler, IMenuUserInterfacePainter, IKeyReleaseHandler
{
    public Feature World { get; }
    public Feature OnMap { get; }
    public Feature UiOutlines { get; }
    public Feature Developer { get; }

    public static bool IsDeveloper { get; private set; }

    private DebugActors[] _debugActors;
    private readonly List<string> _debugLines = new();
    private readonly IFont _uiFont = Render.GetFont(255, 255, 255, 0, "consolas", italic: true, wordWrap: true, shadowMode: FontShadowMode.Heavy);
    private readonly ILineStyle _uiLine = Render.GetLineStyle(255, 255, 255, 0);
    private readonly HashSet<string> _controlsToSkip = new(StringComparer.OrdinalIgnoreCase) { "RTCDebugText_main", "ObjectiveTracker", "Chat", };

    // world
    public bool ShowPlayerSkills { get; private set; }
    public bool ShowGenericActors { get; private set; } = true;
    public bool ShowGizmoActors { get; private set; } = true;
    public bool ShowMonsterActors { get; private set; } = true;
    public bool ShowMonsterAmbientActors { get; private set; }
    public bool ShowNpcActors { get; private set; }
    public bool ShowItems { get; private set; } = true;
    public bool ShowItemsLine { get; private set; }
    public bool ShowName { get; private set; }

    public bool ShowActorFrame { get; private set; }

    // map
    public bool ShowQuests { get; private set; }
    public bool ShowQuestNames { get; private set; }
    public bool ShowGlobalMarkers { get; private set; }
    public bool ShowGlobalMarkerNames { get; private set; }

    public Debug() : base(PluginCategory.Utility, "displays debug information when debug overlay (F11) is turned on")
    {
        Order = int.MaxValue;
        World = AddFeature(nameof(World), "`world")
            .AddBooleanResource(nameof(ShowPlayerSkills), "`player skills",
                () => ShowPlayerSkills, v => ShowPlayerSkills = v)
            .AddBooleanResource(nameof(ShowGenericActors), "`generic actors",
                () => ShowGenericActors, v => ShowGenericActors = v)
            .AddBooleanResource(nameof(ShowGizmoActors), "`gizmo actors",
                () => ShowGizmoActors, v => ShowGizmoActors = v)
            .AddBooleanResource(nameof(ShowMonsterActors), "`monster actors",
                () => ShowMonsterActors, v => ShowMonsterActors = v)
            .AddBooleanResource(nameof(ShowMonsterAmbientActors), "`monster ambient actors",
                () => ShowMonsterAmbientActors, v => ShowMonsterAmbientActors = v)
            .AddBooleanResource(nameof(ShowNpcActors), "`NPC actors",
                () => ShowNpcActors, v => ShowNpcActors = v)
            .AddBooleanResource(nameof(ShowItems), "`items",
                () => ShowItems, v => ShowItems = v)
            .AddBooleanResource(nameof(ShowItemsLine), "`items line",
                () => ShowItemsLine, v => ShowItemsLine = v)
            .AddBooleanResource(nameof(ShowName), "`name",
                () => ShowName, v => ShowName = v)
            .AddBooleanResource(nameof(ShowActorFrame), "`actor frame",
                () => ShowActorFrame, v => ShowActorFrame = v);
        OnMap = AddFeature(nameof(OnMap), "`map")
            .AddBooleanResource(nameof(ShowQuests), "`quests",
                () => ShowQuests, v => ShowQuests = v)
            .AddBooleanResource(nameof(ShowQuestNames), "`quest names",
                () => ShowQuestNames, v => ShowQuestNames = v)
            .AddBooleanResource(nameof(ShowGlobalMarkers), "`global markers",
                () => ShowGlobalMarkers, v => ShowGlobalMarkers = v)
            .AddBooleanResource(nameof(ShowGlobalMarkerNames), "`global marker names",
                () => ShowGlobalMarkerNames, v => ShowGlobalMarkerNames = v);
        UiOutlines = AddFeature(nameof(UiOutlines), "`UI controls");
        Developer = AddFeature(nameof(Developer), "`developer")
            .AddBooleanResource(nameof(IsDeveloper), "`troubadour developer session",
                () => IsDeveloper, v => IsDeveloper = v);
    }

    public void HandleKeyRelease(DirectKey key)
    {
        if (key == DirectKey.F12)
        {
            IsDeveloper = !IsDeveloper;
            return;
        }

        if (!Host.DebugEnabled)
            return;
        var index = key switch
        {
            DirectKey.NumberPad0 => 42,
            DirectKey.NumberPad1 => 0,
            DirectKey.NumberPad2 => 1,
            DirectKey.NumberPad3 => 2,
            DirectKey.NumberPad4 => 3,
            DirectKey.NumberPad5 => 4,
            DirectKey.NumberPad6 => 5,
            DirectKey.NumberPad7 => 6,
            DirectKey.NumberPad8 => 7,
            DirectKey.NumberPad9 => 8,
            _ => -1,
        };
        if (index == 42)
        {
            ShowPlayerSkills = !ShowPlayerSkills;
            return;
        }

        if (index < 0 || index >= _debugActors.Length)
            return;

        var debugActors = _debugActors[index];
        debugActors.Toggle.Invoke();
    }

    public bool BeforeRender()
    {
        if (!Host.DebugEnabled)
            return true;

        _debugLines.Clear();
        return true;
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (!Host.DebugEnabled)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                foreach (var actors in GetDebugActorsArray())
                {
                    Actors(actors);
                }

                if (ShowActorFrame && _debugLines.Any())
                {
                    DrawDebugFrame(_debugLines, (Game.WindowWidth * 0.125f) + 20f, 0);
                }

                break;
            case GameWorldLayer.Map:
                if (ShowQuests)
                {
                    foreach (var questSno in GameData.AllQuestSno)
                    {
                        var isOnMap = Map.WorldToMapCoordinate(questSno.WorldCoordinate, out var mapX, out var mapY);
                        /*if (questSno.SnoId == QuestSnoId.LE_MonsterWaves_Standard_Update)
                        {
                            var isOnMap2 = Map.WorldToMapCoordinate(questSno.ActivationWorldCoordinate, out var mapX2, out var mapY2);
                            var t = $"""
{questSno.SnoId}
{mapX} {mapY} {isOnMap}
{mapX2} {mapY2} {isOnMap2}
""";
                            DrawDebugFrame(t, Game.WindowWidth / 2f, Game.WindowHeight / 2f);
                        }//*/

                        if (!isOnMap)
                            continue;

                        DrawDebugFrame(ShowQuestNames ? questSno.NameLocalized : questSno.SnoId.ToString(), mapX, mapY);
                    }
                }

                if (ShowGlobalMarkers)
                {
                    foreach (var marker in Game.GlobalMarkers)
                    {
                        if (!Map.WorldToMapCoordinate(marker.WorldCoordinate, out var mapX, out var mapY))
                            continue;

                        var text = ShowGlobalMarkerNames ? marker.ActorSno?.NameLocalized : marker.ActorSno?.SnoId.ToString();
                        DrawDebugFrame(text, mapX, mapY);
                    }
                }

                break;
        }
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (!Host.DebugEnabled || layer != GameUserInterfaceLayer.AfterClip)
            return;

        Game.Items.FirstOrDefault(x => x.IsSelected)?.SetHint(this);
        UserInterfaceOutlines();
        DrawOptionsFrame();
        MySkills();
    }

    public void PaintMenuUserInterface()
    {
        /*
        var t2 = GameData.AllActorSno
            .Where(x =>
            {
                var sno = x.SnoId.ToString();
                return sno.StartsWith("Carryable_") || sno.StartsWith("Global_Flippy_Items_");
            })
            .Select(x => $"{x.SnoId,-40}{x.ItemUseType,-10}{x.NameLocalized}")
            .ToArray();
        DrawDebugFrame(t2.Length + Environment.NewLine + string.Join(Environment.NewLine, t2), 0, 0);
        //*/

        if (!Host.DebugEnabled)
            return;

        UserInterfaceOutlines();
        DrawOptionsFrame();
    }

    private void DrawOptionsFrame()
    {
        if (!ShowActorFrame)
            return;

        var optionLines = new List<string> { $"[{(ShowPlayerSkills ? "X" : " ")}] (0) MySkills", };
        optionLines.AddRange(GetDebugActorsArray().Take(9).Select(x => $"[{(x.Enabled.Invoke() ? "X" : " ")}] ({x.Index}) {x.Name}"));
        DrawDebugFrame(optionLines, Game.WindowWidth * 0.125f, 0, true);
    }

    private void UserInterfaceOutlines()
    {
        if (!UiOutlines.Enabled)
            return;

        var controls = UserInterface.AllRegisteredControls
            .Where(x => x.Visible && x.Path != null)
            .Where(x => !_controlsToSkip.Contains(x.Path));
        foreach (var control in controls)
        {
            var mouseX = Game.CursorX;
            var mouseY = Game.CursorY;
            if (mouseX < control.Left
                || mouseX > control.Left + control.Width
                || mouseY < control.Top
                || mouseY > control.Top + control.Height
               ) continue;

            _uiLine.DrawRectangle(control.Left, control.Top, control.Width, control.Height);
            var tl = _uiFont.GetTextLayout($"{control.Path} {control.Width:#.#}x{control.Height:#.#}", control.Width);
            tl.DrawText(control.Left + control.Width - tl.Width, control.Top + control.Height - tl.Height);
        }
    }

    private void Actors(DebugActors debugActors)
    {
        if (!debugActors.Enabled.Invoke())
            return;

        foreach (var actor in debugActors.Actors.Invoke())
        {
            if (debugActors.LineStyle is not null && debugActors.LineEnabled.Invoke())
                debugActors.LineStyle.DrawWorldLine(actor.Coordinate, Game.MyPlayerActor.Coordinate, sharpen: false);
            if (actor.Coordinate.IsOnScreen)
                debugActors.CircleStyle.DrawWorldEllipse(0.5f, -1, actor.Coordinate, sharpen: false);
            string name;
            if (actor.ActorSno.SnoId == ActorSnoId.Generic_Proxy)
            {
                name = $"GenericProxy {actor.AcdId} {actor.AnnId}";
                var tl1 = DebugFont.GetTextLayout(name);
                tl1.DrawText(actor.Coordinate.ScreenX - (tl1.Width / 2f), actor.Coordinate.ScreenY - (tl1.Height / 2f));
            }
            else
            {
                name = ShowName ? actor.ActorSno.NameEnglish : actor.ActorSno.SnoId.ToString();
                if (actor is IMonsterActor monster)
                {
                    name = $"{name} {monster.MonsterData.ArcheType.MonsterFamilySnoId}";
                }

                if (!string.IsNullOrEmpty(name))
                {
                    var tl = DebugFont.GetTextLayout(name);
                    tl.DrawText(actor.Coordinate.ScreenX - (tl.Width / 2), actor.Coordinate.ScreenY - (tl.Height / 2));
                }
            }

            if (ShowActorFrame)
            {
                _debugLines.Add($"{debugActors.Name}: {name}");
            }
        }
    }

    public void MySkills()
    {
        if (!ShowPlayerSkills)
            return;

        var lines = Game.MyPlayer.Skills.Select(x => $"""
{x.PowerSno.SnoId}
CD: {x.CooldownStartTick} {x.CooldownEndTick}
Charges: {x.SkillCharges} {x.NextChargeTick} {x.RechargeStartTick}

""").ToArray();
        DrawDebugFrame(lines, 0, Game.WindowHeight * 0.125f);
    }

    private DebugActors[] GetDebugActorsArray()
    {
        if (_debugActors != null)
            return _debugActors;
        _debugActors = new DebugActors[]
        {
            new()
            {
                Name = "Generic",
                Enabled = () => ShowGenericActors,
                Toggle = () => ShowGenericActors = !ShowGenericActors,
                CircleStyle = Render.GetLineStyle(128, 255, 255, 255),
                Actors = () => Game.GenericActors.Where(x => x.Coordinate.IsOnScreen
                                                             && x.ActorSno.SnoId != ActorSnoId.Generic_Proxy
                                                             && x.ActorSno.SnoId != ActorSnoId.Symbol_Quest
                                                             && x.ActorSno.SnoId != ActorSnoId.Symbol_Quest_Proxy
                                                             && x.ActorSno.SnoId != ActorSnoId.MarkerLocation
                )
            },
            new()
            {
                Name = "Gizmo",
                Enabled = () => ShowGizmoActors,
                Toggle = () => ShowGizmoActors = !ShowGizmoActors,
                CircleStyle = Render.GetLineStyle(128, 180, 180, 255),
                Actors = () => Game.GizmoActors.Where(x => x.Coordinate.IsOnScreen)
            },
            new()
            {
                Name = "Monster",
                Enabled = () => ShowMonsterActors,
                Toggle = () => ShowMonsterActors = !ShowMonsterActors,
                CircleStyle = Render.GetLineStyle(255, 255, 64, 64),
                Actors = () => Game.Monsters.Where(x => x.Coordinate.IsOnScreen
                                                        && !x.IsNPC
                                                        && x.MonsterData.ArcheType.MonsterFamilySnoId != MonsterFamilySnoId.ambient
                )
            },
            new()
            {
                Name = "Ambient",
                Enabled = () => ShowMonsterAmbientActors,
                Toggle = () => ShowMonsterAmbientActors = !ShowMonsterAmbientActors,
                CircleStyle = Render.GetLineStyle(255, 255, 64, 64),
                Actors = () => Game.Monsters.Where(x => x.Coordinate.IsOnScreen
                                                        && x.MonsterData.ArcheType.MonsterFamilySnoId == MonsterFamilySnoId.ambient
                )
            },
            new()
            {
                Name = "NPC",
                Enabled = () => ShowNpcActors,
                Toggle = () => ShowNpcActors = !ShowNpcActors,
                CircleStyle = Render.GetLineStyle(255, 255, 64, 64),
                Actors = () => Game.Monsters.Where(x => x.Coordinate.IsOnScreen
                                                        && x.IsNPC
                                                        && x.MonsterData.ArcheType.MonsterFamilySnoId != MonsterFamilySnoId.ambient
                )
            },
            new()
            {
                Name = "Item",
                Enabled = () => ShowItems,
                Toggle = () => ShowItems = !ShowItems,
                CircleStyle = Render.GetLineStyle(255, 64, 160, 64, width: 3),
                LineEnabled = () => ShowItemsLine,
                LineStyle = Render.GetLineStyle(255, 64, 160, 64, width: 3),
                Actors = () => Game.Items.Where(x => x.Location == ItemLocation.None
                                                     && x.ItemSno.ItemTypeSno.SnoId != ItemTypeSnoId.Gold
                                                     && x.ItemSno.ItemTypeSno.SnoId != ItemTypeSnoId.HealthPotionDosePickUp
                )
            }
        };
        var idx = 1;
        foreach (var actor in _debugActors)
        {
            actor.Index = idx;
            idx++;
        }

        return _debugActors;
    }
}

public sealed class DebugActors
{
    public int Index { get; set; }
    public string Name { get; init; }
    public Func<IEnumerable<ICommonActor>> Actors { get; init; }
    public Func<bool> Enabled { get; init; }
    public Func<bool> Toggle { get; init; }
    public ILineStyle CircleStyle { get; init; }
    public Func<bool> LineEnabled { get; init; }
    public ILineStyle LineStyle { get; init; }
}