namespace T4.Plugins.Troubadour;

public sealed class GetOut : TroubadourPlugin, IGameWorldPainter
{
    public Feature Config { get; }
    public Feature Developer { get; }

    public static IFont TrapFont { get; } = Render.GetFont(240, 255, 255, 255, "consolas");
    public static ILineStyle LineStyle { get; } = Render.GetLineStyle(255, 255, 0, 0, DashStyle.Dash);

    public bool OnMonsters { get; set; }
    public bool OnGizmos { get; set; }
    public bool OnGenerics { get; set; }
    public bool OnItems { get; set; }

    public GetOut() : base(PluginCategory.Fight, "displays dangerous affixes on ground.")
    {
        EnabledByDefault = false;
        Config = AddFeature(nameof(Config), "config")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style");
        Developer = AddFeature(nameof(Developer), "developer")
            .AddBooleanResource(nameof(OnMonsters), "`on monsters", () => OnMonsters, v => OnMonsters = v)
            .AddBooleanResource(nameof(OnGizmos), "`on gizmos", () => OnGizmos, v => OnGizmos = v)
            .AddBooleanResource(nameof(OnGenerics), "`on generics", () => OnGenerics, v => OnGenerics = v)
            .AddBooleanResource(nameof(OnGenerics), "`on items", () => OnItems, v => OnItems = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (layer != GameWorldLayer.Ground)
            return;

        DrawActors(true, Game.Monsters.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
        DrawActors(true, Game.GizmoActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));
        DrawActors(true, Game.GenericActors.Where(x => ActorSnoIdSet.Contains(x.ActorSno.SnoId)));

        if (!Developer.Enabled)
            return;

        DrawDevActors(x => DevActorSnoIdSet.Contains(x.ActorSno.SnoId), OnMonsters, OnGizmos, OnGenerics, OnItems);
    }

    public static void DrawActors(bool enabled, IEnumerable<ICommonActor> actors)
    {
        if (!enabled)
            return;

        foreach (var actor in actors)
        {
            var radius = actor.GetWorldRadius();
            LineStyle.DrawWorldEllipse(radius, -1, actor.Coordinate, false, strokeWidthCorrection: 2f);
        }
    }

    private HashSet<ActorSnoId> ActorSnoIdSet { get; } = new()
    {
        // spikes 1.3f
        ActorSnoId.DRLG_Trap_Spikes_2x2,
        ActorSnoId.DRLG_Trap_Spikes_3x3,
        // barrels 0.5f
        ActorSnoId.DRLG_Trap_Barrel_Cold_AOE,
        ActorSnoId.DRLG_Trap_Barrel_Fire_AOE,
        ActorSnoId.DRLG_Trap_Barrel_Lightning_AOE,
        ActorSnoId.DRLG_Trap_Barrel_Poison_AOE,
        ActorSnoId.DRLG_Trap_Barrel_Shadow_AOE,
        // traps 1f
        // ActorSnoId.DRLG_Trap_BearTrap,
        ActorSnoId.DRLG_Trap_BearTrap_DestroyableObject,
        ActorSnoId.DRLG_Trap_Moonseed,
        // mines 1f
        ActorSnoId.DRLG_Trap_Mine_Cold,
        ActorSnoId.DRLG_Trap_Mine_Fire,
        ActorSnoId.DRLG_Trap_Mine_Lightning,
        ActorSnoId.DRLG_Trap_Mine_Poison,
        ActorSnoId.DRLG_Trap_Mine_Shadow,
        // falls 1f
        ActorSnoId.DRLG_Trap_FallingRocks_FallingRocks,
        ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksBrickDark,
        ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksBrickDarkAndWood,
        ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksBrickLight,
        ActorSnoId.DRLG_Trap_FallingRocks_FallingRocksSand,
        // dungeon affixes
        ActorSnoId.Dungeon_Affix_Orb,
        ActorSnoId.dungeon_affix_driftingShade_payload_burst_coasters,
        ActorSnoId.dungeon_affix_driftingShade_payload_persistent_ringMesh,
        ActorSnoId.dungeon_affix_driftingShade_projectile,
        ActorSnoId.dungeon_affix_driftingShade_projectile_coreMesh,
        ActorSnoId.dungeon_affix_driftingShade_projectile_parent,
        ActorSnoId.dungeon_affix_driftingShade_warning_coasters,
        //
        ActorSnoId.DRLG_Trap_Demonic,
    };

    private HashSet<ActorSnoId> DevActorSnoIdSet { get; } = new()
    {
        // traps
        ActorSnoId.DRLG_Trap_BearTrap_BanditBoss,
        ActorSnoId.DRLG_Trap_Demonic_Projectile,
        ActorSnoId.DRLG_Trap_Frozen,
        ActorSnoId.DRLG_Trap_Frozen_Projectile,
        ActorSnoId.DRLG_Trap_Frozen_Projectile_Initial,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Cold_Stage01_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Cold_Stage02_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Fire_Stage01_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Fire_Stage02_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Lightning_Stage01_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Lightning_Stage02_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Poison_Stage01_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Poison_Stage02_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Shadow_Stage01_Dyn,
        ActorSnoId.DRLG_Trap_Generic_ElementalTotem_01_Shadow_Stage02_Dyn,
        ActorSnoId.DRLG_Trap_Geyser_Hell,
        ActorSnoId.DRLG_Trap_geyser_hell_deploy_bubble_mesh,
        ActorSnoId.DRLG_Trap_Geyser_Hell_Deploy_glowSphere,
        ActorSnoId.DRLG_Trap_Gloomspore,
        ActorSnoId.DRLG_Trap_Log,
        ActorSnoId.DRLG_Trap_Log_DestroyableObject,
        ActorSnoId.DRLG_Trap_Spectral,
        ActorSnoId.DRLG_Trap_Spectral_Projectile,
        ActorSnoId.DRLG_Trap_Spectral_Projectile_First,
        ActorSnoId.DRLG_Trap_Totem_Fire,
        ActorSnoId.DRLG_Trap_Totem_Ice,
        ActorSnoId.DRLG_Trap_Totem_Lightning,
        ActorSnoId.DRLG_Trap_Totem_Poison,
        ActorSnoId.DRLG_Trap_Totem_Shadow,

        // affixes
        ActorSnoId.monsterAffix_chainPillar_base_chain,
        ActorSnoId.monsterAffix_Chaos_fumeRing,
        ActorSnoId.MonsterAffix_Chaos_WarningRing,
        ActorSnoId.MonsterAffix_ChillingWind_Wall,
        ActorSnoId.monsterAffix_electricLance_projectile_actor,
        ActorSnoId.monsterAffix_fireEnchanted_death_projectile,
        ActorSnoId.monsterAffix_FireEnchanted_flameTube,
        ActorSnoId.MonsterAffix_FireEnchanted_impact_glowsphere,
        ActorSnoId.monsterAffix_FireEnchanted_pjt,
        ActorSnoId.monsterAffix_FireEnchanted_pjt_leadingedge,
        ActorSnoId.MonsterAffix_FireEnchanted_Projectile,
        ActorSnoId.monsterAffix_fireEnchanted_warning_swirl,
        ActorSnoId.monsterAffix_fireEnchanted_wave_shell,
        ActorSnoId.monsterAffix_FireOrbs,
        ActorSnoId.MonsterAffix_FireOrbs_active,
        ActorSnoId.MonsterAffix_FireOrbs_Orb,
        ActorSnoId.monsterAffix_frozen_aoe_explosion_frostBuildupSphere,
        ActorSnoId.MonsterAffix_Frozen_Circle,
        ActorSnoId.monsterAffix_frozen_orb_mesh,
        ActorSnoId.monsterAffix_legendary_lightning_buildup_meshRoundBolt,
        ActorSnoId.monsterAffix_lightningEnchanted_projectile,
        ActorSnoId.monsterAffix_lightningEnchanted_trailActor,
        ActorSnoId.monsterAffix_lightningTotems_proj,
        ActorSnoId.monsterAffix_lightningTotems_projectile,
        ActorSnoId.MonsterAffix_LightningTotems_Totem,
        ActorSnoId.monsterAffix_mortar_aoe_explosion_FillSphere,
        ActorSnoId.monsterAffix_mortar_burstRing,
        ActorSnoId.monsterAffix_mortar_lobbed_attractor_child_proxy,
        ActorSnoId.monsterAffix_mortar_lobbed_attractor_parent_proxy,
        ActorSnoId.monsterAffix_mortar_projectile,
        ActorSnoId.monsterAffix_mortar_projectile_mesh,
        ActorSnoId.monsterAffix_mortar_warning_cyl,
        ActorSnoId.monsterAffix_plagueBearer_aoeExplosion_bubble,
        ActorSnoId.monsterAffix_plagueBearer_omni,
        ActorSnoId.MonsterAffix_PoisonEnchanted_Proxy,
        ActorSnoId.monsterAffix_poisonEnchantedoutwardMesh,
        ActorSnoId.monsterAffix_shadow_introSwirl,
        ActorSnoId.MonsterAffix_Summoner_Circle,
        ActorSnoId.MonsterAffix_Summoner_Circle_Small,
        ActorSnoId.monsterAffix_summoner_spawn_portal,
        ActorSnoId.monsterAffix_suppressor_barrier,
        ActorSnoId.monsterAffix_suppressor_parabola,
        ActorSnoId.monsterAffix_tether_chainBreak_links,
        ActorSnoId.MonsterAffix_Tether_Pillar,
        ActorSnoId.MonsterAffix_Tether_Pillar_01_Client_01_Dyn,
        ActorSnoId.MonsterAffix_Tether_Pillar_01_runeChains_ring,
        ActorSnoId.MonsterAffix_Tether_Pillar_AutoChain,
        ActorSnoId.monsterAffix_vampiricEnrage_powerUp,
        ActorSnoId.MonsterAffix_Vortex,
        ActorSnoId.monsterAffix_vortex_warning_icicle,
        ActorSnoId.monsterAffix_vortex_warning_icicle_fractured,
        // waller
        // ActorSnoId.MonsterAffix_Waller_damaged_Dyn,
        // ActorSnoId.MonsterAffix_Waller_Damaged_Multistage,
        // ActorSnoId.monsterAffix_waller_Impact_RockBreak,
        // ActorSnoId.MonsterAffix_Waller_rock_A_Dyn,
        // ActorSnoId.MonsterAffix_Waller_rock_B_Dyn,
        // ActorSnoId.MonsterAffix_Waller_rock_C_Dyn,
        // ActorSnoId.MonsterAffix_Waller_rock_E_Dyn,
        // ActorSnoId.MonsterAffix_Waller_rock_F_Dyn,
        // ActorSnoId.MonsterAffix_Waller_rock_G_Dyn,
        // ActorSnoId.MonsterAffix_Waller_rock_H_Dyn,
        // ActorSnoId.MonsterAffix_Waller_Untargetable_Wall,
        // ActorSnoId.MonsterAffix_Waller_Untargetable_Wall_Cannibal_Tyrant,
    };
}