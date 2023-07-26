/*
namespace T4.Plugins.Troubadour;

public sealed class HelltidesDebug : JackPlugin, IGameWorldPainter
{
    public Feature Developer { get; private set; }

    public int Hour { get; set; }
    public float OffsetX { get; set; }
    public float OffsetY { get; set; }
    public ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 255, 0, 0);
    public ILineStyle LineStyle2 { get; } = Render.GetLineStyle(255, 255, 255, 0);
    public IFillStyle FillStyle { get; } = Render.GetFillStyle(255, 255, 255, 0);

    private TimeZoneInfo _pstTimeZoneInfo;

    public HelltidesDebug() : base(PluginCategory.Utility, "`Helltide companion debugger")
    {
        Order = -1;
        EnabledByDefault = false;
        Developer = AddFeature( nameof(Developer), "Developer")
                .AddFloatResource( nameof(Hour), "`Fake Hour", 0, 23, () => Hour, v => Hour = (int)Math.Floor(v))
                .AddFloatResource( nameof(OffsetX), "`Offset X", -2000, 2000, () => OffsetX, v => OffsetX = v)
                .AddFloatResource( nameof(OffsetY), "`Offset Y", -2000, 2000, () => OffsetY, v => OffsetY = v);
        Try(() => _pstTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time"));
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Map)
            return;

        var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _pstTimeZoneInfo);
        DrawDebug();

        var helltide = Game.HelltideEventMarkers.FirstOrDefault(x => x.EndsInMilliseconds > 0);
        if (helltide is null)
            return;

        foreach (var chest in Game.GizmoActors.Where(x => x.ActorSno.SnoId == ActorSnoId.usz_rewardGizmo_Uber))
        {
            LineStyle2.DrawWorldEllipse(1f, -1, chest.Coordinate, strokeWidthCorrection: 2f);
        }

        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(helltide.SubzoneSno.SnoId, now.Hour))
        {
            DrawChest(chest, out var mapX, out var mapY);
        }
    }

    private void DrawDebug()
    {
        foreach (var chest in GetAllMysteriousChests())
        {
            if (!Map.WorldToMapCoordinate(chest.X, chest.Y, out var mapX, out var mapY))
                continue;

            // Textures.HelltideChest.Draw(mapX, mapY, 10, 10);
            LineStyle.DrawEllipse(mapX, mapY, 5f, 5f, strokeWidthCorrection: 2f);
            var text = $"{chest.Tag}\n{chest.X:#.00} {chest.Y:#.00}";
            DrawDevText(() => text, mapX, mapY);
        }

        var fakeNow = new DateTime(2023, 7, 15, Hour, 0, 0, DateTimeKind.Local);
        var fakeNowUtc = fakeNow.ToUniversalTime();
        var fakeNowPst = TimeZoneInfo.ConvertTimeFromUtc(fakeNowUtc, _pstTimeZoneInfo);
        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(SubzoneSnoId.Frac_Tundra_S, fakeNowPst.Hour))
        {
            DrawChest(chest, out _, out _);
        }

        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(SubzoneSnoId.Scos_ZoneEvent, fakeNowPst.Hour))
        {
            DrawChest(chest, out _, out _);
        }

        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(SubzoneSnoId.Step_TempleOfRot, fakeNowPst.Hour))
        {
            DrawChest(chest, out _, out _);
        }

        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(SubzoneSnoId.Hawe_ZoneEvent, fakeNowPst.Hour))
        {
            DrawChest(chest, out _, out _);
        }

        foreach (var chest in HelltidesStore.GetActiveMysteriousChests(SubzoneSnoId.Kehj_ZoneEvent, fakeNowPst.Hour))
        {
            DrawChest(chest, out _, out _);
        }
    }

    private void DrawChest(HelltideChest chest, out float mapX, out float mapY)
    {
        if (!Map.WorldToMapCoordinate(chest.X, chest.Y, out mapX, out mapY))
            return;

        const float size = 40f;
        const float halfSize = size / 2f;
        const float radius = halfSize * 0.6f;
        // FillStyle.FillEllipse(mapX, mapY, halfSize, halfSize);
        FillStyle.FillEllipse(mapX, mapY, radius, radius);
        // LineStyle2.DrawEllipse(mapX, mapY, radius, radius, strokeWidthCorrection: 1.5f);
        Textures.HelltideChest.Draw(mapX - halfSize, mapY - halfSize, size, size);
        // LineStyle.DrawEllipse(mapX, mapY, 5f, 5f);
    }

    public static IEnumerable<HelltideChest> GetAllMysteriousChests()
    {
        foreach (var chest in HelltidesStore.FP_G1_1A_2B)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.FP_G2_1B_2A)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.SG_G1_1A_2D)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.SG_G2_1D_2A)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.SG_G3_1C_2B)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.SG_G4_1B_2C)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.DS_G1_1A_2B)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.DS_G2_1B_2A)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.HZ_G1_1A_2B)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.HZ_G2_1B_2A)
        {
            yield return chest;
        }

        foreach (var chest in HelltidesStore.KJ_G1_1A_1B_1C)
        {
            yield return chest;
        }

        yield return HelltidesStore.KJ_G2_2A;
        yield return HelltidesStore.KJ_G2_3A;
        yield return HelltidesStore.KJ_G3_2B;
        yield return HelltidesStore.KJ_G3_3B;
    }
}
//*/