namespace T4.Plugins.Troubadour;

public sealed class HelltideChestIconsFeature : WorldFeature<ICommonActor>
{
    private readonly ITexture _backgroundTexture = Render.GetTexture(SupportedTextureId.UIInventoryIcons_1144197672);
    private readonly ITexture _helltideChest = Render.GetTexture(SupportedTextureId.UIMinimapIcons_43109186);

    private HelltideChestIconsFeature()
    {
        Enabled = false;

        LineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapLineStyle = Render.GetLineStyle(200, 255, 255, 0);
        MapIconSize = 32f;
        WorldIconTexture = _helltideChest;
        WorldIconSize = 64f;
        MapIconTexture = _helltideChest;

        _uberChests = _uberChestsTextures.Keys.ToHashSet();
    }

    public override IEnumerable<ICommonActor> GetWorldActors()
    {
        return Game.GizmoActors.Where(x => _uberChests.Contains(x.ActorSno.SnoId));
    }

    public static HelltideChestIconsFeature Create(IPlugin plugin, string nameOf)
    {
        var feature = new HelltideChestIconsFeature
        {
            Plugin = plugin,
            NameOf = nameOf,
            DisplayName = () => Translation.Translate(plugin, "icon on chests"),
            Resources = new List<AbstractFeatureResource>()
        };
        // feature.AddDefaultGroundResources();
        // feature.AddDefaultMapResources();

        plugin.Features.Add(feature);
        return feature;
    }

    public override void PaintGround()
    {
        if (!Enabled || !OnGroundEnabled)
            return;

        foreach (var item in GetWorldActors())
        {
            _uberChestsTextures.TryGetValue(item.ActorSno.SnoId, out var texture);
            if (texture is null) 
                continue;

            var size = WorldIconSize;
            var x = item.Coordinate.ScreenX - (size / 2);
            var y = item.Coordinate.ScreenY - (size / 2);
            _backgroundTexture.Draw(x, y, size, size);
            texture.Draw(x, y, size, size);
        }
    }

    public override void PaintMap()
    {
        if (!Enabled || !OnMapEnabled)
            return;

        foreach (var item in GetWorldActors())
        {
            if (!item.Coordinate.IsOnMap)
                continue;

            _uberChestsTextures.TryGetValue(item.ActorSno.SnoId, out var texture);
            if (texture is null) 
                continue;

            var size = MapIconSize;
            var x = item.Coordinate.MapX - (size / 2);
            var y = item.Coordinate.MapY - (size / 2);
            _backgroundTexture.Draw(x, y, size, size);
            texture.Draw(x, y, size, size);
        }
    }

    private readonly HashSet<ActorSnoId> _uberChests;

    private readonly Dictionary<ActorSnoId, ITexture> _uberChestsTextures = new()
    {
        // [ActorSnoId.usz_rewardGizmo_1H] = Textures.Sword_TBN,
        // [ActorSnoId.usz_rewardGizmo_2H] = Textures.CrossedSwordsOnShield,
        // [ActorSnoId.usz_rewardGizmo_Amulet] = Textures.Amulet,
        // [ActorSnoId.usz_rewardGizmo_Boots] = Textures.Boots,
        // [ActorSnoId.usz_rewardGizmo_ChestArmor] = Textures.Sword_TBN,
        // [ActorSnoId.usz_rewardGizmo_Gloves] = Textures.Gloves,
        // [ActorSnoId.usz_rewardGizmo_Helm] = Textures.Helm,
        // [ActorSnoId.usz_rewardGizmo_Legs] = Textures.Pants,
        // [ActorSnoId.usz_rewardGizmo_Rings] = Textures.Ring,
        // [ActorSnoId.usz_rewardGizmo_Uber] = Textures.TreasureBag,
    };
}
// public static ITexture CrossedSwordsOnShield { get; } = Render.GetTexture(2089728434u);
// public static ITexture TreasureBag { get; } = Render.GetTexture(2044228644u);
// public static ITexture Amulet { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_050);
// public static ITexture Boots { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_051);
// public static ITexture Chest { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_052);
// public static ITexture Gloves { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_053);
// public static ITexture Helm { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_054);
// public static ITexture Pants { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_055);
// public static ITexture Offhand { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_056);
// public static ITexture Ring { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_057);
//
// public static ITexture Sword_TBN { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_093);
// public static ITexture Bow { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_095);
// public static ITexture Offhand2 { get; } = Render.GetTexture(SupportedTextureId.UIFontIcon_097);