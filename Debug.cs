namespace T4.Plugins.Troubadour;

public sealed class Debug : BasePlugin, IGameWorldPainter, IGameUserInterfacePainter, IRenderEnabler, IMenuUserInterfacePainter, IKeyReleaseHandler
{
    public Feature Config { get; private set; }

    private DebugActors[] _debugActors;
    private readonly List<string> _debugLines = new();
    private readonly IFont _uiFont = Render.GetFont(255, 255, 255, 0, "consolas", italic: true, wordWrap: true, shadowMode: FontShadowMode.Heavy);
    private readonly ILineStyle _uiLine = Render.GetLineStyle(255, 255, 255, 0);
    private readonly HashSet<string> _controlsToSkip = new(StringComparer.OrdinalIgnoreCase) { "RTCDebugText_main", "ObjectiveTracker", "Chat", };

    public bool ShowPlayerSkills { get; set; } = true;
    public bool ShowGenericActors { get; set; } = true;
    public bool ShowGyzmoActors { get; set; } = true;
    public bool ShowMonsterActors { get; set; } = true;
    public bool ShowNpcActors { get; set; }
    public bool ShowItems { get; set; } = true;
    public bool ShowItemsLine { get; set; }
    public bool ShowName { get; set; }
    public bool ShowActorFrame { get; set; }
    public bool ShowUserInterfaceControls { get; set; }

    public Debug()
    {
        Order = int.MaxValue;
        EnabledByDefault = IsDevSession;
    }

    public override string GetDescription() => Translation.Translate(this, "displays debug information when debug overlay (F11) is turned on");

    public override void Load()
    {
        Config = new Feature
        {
            Plugin = this,
            NameOf = nameof(Config),
            DisplayName = () => nameof(Config),
            Resources = new List<AbstractFeatureResource>
            {
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowPlayerSkills),
                    DisplayText = () => Translation.Translate(this, "player skills"),
                    Getter = () => ShowPlayerSkills,
                    Setter = value => ShowPlayerSkills = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowGenericActors),
                    DisplayText = () => Translation.Translate(this, "generic actors"),
                    Getter = () => ShowGenericActors,
                    Setter = value => ShowGenericActors = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowGyzmoActors),
                    DisplayText = () => Translation.Translate(this, "gyzmo actors"),
                    Getter = () => ShowGyzmoActors,
                    Setter = value => ShowGyzmoActors = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowMonsterActors),
                    DisplayText = () => Translation.Translate(this, "monster actors"),
                    Getter = () => ShowMonsterActors,
                    Setter = value => ShowMonsterActors = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowNpcActors),
                    DisplayText = () => Translation.Translate(this, "NPC actors"),
                    Getter = () => ShowNpcActors,
                    Setter = value => ShowNpcActors = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowItems),
                    DisplayText = () => Translation.Translate(this, "items"),
                    Getter = () => ShowItems,
                    Setter = value => ShowItems = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowItemsLine),
                    DisplayText = () => Translation.Translate(this, "items line"),
                    Getter = () => ShowItemsLine,
                    Setter = value => ShowItemsLine = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowName),
                    DisplayText = () => Translation.Translate(this, "name"),
                    Getter = () => ShowName,
                    Setter = value => ShowName = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowActorFrame),
                    DisplayText = () => Translation.Translate(this, "actor frame"),
                    Getter = () => ShowActorFrame,
                    Setter = value => ShowActorFrame = value,
                },
                new BooleanFeatureResource
                {
                    NameOf = nameof(ShowUserInterfaceControls),
                    DisplayText = () => Translation.Translate(this, "UI controls"),
                    Getter = () => ShowUserInterfaceControls,
                    Setter = value => ShowUserInterfaceControls = value,
                },
            }
        }.Register();
    }

    public void HandleKeyRelease(DirectKey key)
    {
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
        if (layer != GameWorldLayer.Ground || !Host.DebugEnabled)
            return;

        foreach (var actors in GetDebugActorsArray())
        {
            Actors(actors);
        }

        if (ShowActorFrame && _debugLines.Any())
        {
            DrawDebugFrame(_debugLines, (Game.WindowWidth * 0.125f) + 20f, 0);
        }
    }

    public void PaintGameUserInterface(GameUserInterfaceLayer layer)
    {
        if (layer != GameUserInterfaceLayer.AfterClip || !Host.DebugEnabled)
            return;

        Game.Items.FirstOrDefault(x => x.IsSelected)?.SetHint(this);
        UserInterfaceOutlines();
        DrawOptionsFrame();
        MySkills();
    }

    public void PaintMenuUserInterface()
    {
        if (!Host.DebugEnabled)
            return;

        UserInterfaceOutlines();
        DrawOptionsFrame();
    }

    private void DrawOptionsFrame()
    {
        if (!ShowActorFrame)
            return;

        var optionLines = new List<string>
        {
            $"[{(ShowPlayerSkills ? "X" : " ")}] (0) MySkills",
        };
        optionLines.AddRange(GetDebugActorsArray().Take(9).Select(x => $"[{(x.Enabled.Invoke() ? "X" : " ")}] ({x.Index}) {x.Name}"));
        DrawDebugFrame(optionLines, Game.WindowWidth * 0.125f, 0, true);
    }

    private void UserInterfaceOutlines()
    {
        if (!ShowUserInterfaceControls)
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

            var name = ShowName ? actor.ActorSno.NameEnglish : actor.ActorSno.SnoId.ToString();
            if (!string.IsNullOrEmpty(name))
            {
                var tl = DebugFont.GetTextLayout(name);
                tl.DrawText(actor.Coordinate.ScreenX - (tl.Width / 2), actor.Coordinate.ScreenY - (tl.Height / 2));
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
                Actors = () => Game.GenericActors.Where(x => x.Coordinate.IsOnScreen)
            },
            new()
            {
                Name = "Gizmo",
                Enabled = () => ShowGyzmoActors,
                Toggle = () => ShowGyzmoActors = !ShowGyzmoActors,
                CircleStyle = Render.GetLineStyle(128, 180, 180, 255),
                Actors = () => Game.GizmoActors.Where(x => x.Coordinate.IsOnScreen)
            },
            new()
            {
                Name = "Monster",
                Enabled = () => ShowMonsterActors,
                Toggle = () => ShowMonsterActors = !ShowMonsterActors,
                CircleStyle = Render.GetLineStyle(255, 255, 64, 64),
                Actors = () => Game.Monsters.Where(x => x.Coordinate.IsOnScreen && !x.IsNPC)
            },
            new()
            {
                Name = "NPC",
                Enabled = () => ShowNpcActors,
                Toggle = () => ShowNpcActors = !ShowNpcActors,
                CircleStyle = Render.GetLineStyle(255, 255, 64, 64),
                Actors = () => Game.Monsters.Where(x => x.Coordinate.IsOnScreen && x.IsNPC)
            },
            new()
            {
                Name = "Item",
                Enabled = () => ShowItems,
                Toggle = () => ShowItems = !ShowItems,
                CircleStyle = Render.GetLineStyle(255, 64, 160, 64, width: 3),
                LineEnabled = () => ShowItemsLine,
                LineStyle = Render.GetLineStyle(255, 64, 160, 64, width: 3),
                Actors = () => Game.Items.Where(x => x.Location == ItemLocation.None && x.ItemSno.ItemTypeSno.SnoId != ItemTypeSnoId.Gold)
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

/*
public sealed class BooleanResource : BooleanFeatureResource
{
    public bool Value { get; set; }

    public BooleanResource(string name, bool value = false)
    {
        NameOf = name;
        Value = value;
    }

    public static bool operator true(BooleanResource x) => x.Value;
    public static bool operator false(BooleanResource x) => !x.Value;
}
//*/