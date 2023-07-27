namespace T4.Plugins.Troubadour;

public sealed class DungeonQuests : TroubadourPlugin, IGameWorldPainter
{
    public Feature OnGround { get; }
    public Feature OnMap { get; }
    public Feature Developer { get; }

    public ILineStyle LineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float WorldCircleSize { get; set; } = 0.5f;
    public float WorldCircleStroke { get; set; } = 2f;
    public ILineStyle MapLineStyle { get; } = Render.GetLineStyle(200, 255, 255, 0);
    public float MapCircleSize { get; set; } = 8f;
    public float MapCircleStroke { get; set; } = 4f;

    public bool OnMonsters { get; set; }
    public bool OnGizmos { get; set; }
    public bool OnGenerics { get; set; }
    public bool OnItems { get; set; }

    public DungeonQuests() : base(PluginCategory.Dungeon, "displays dungeon quest objectives on map and ground.")
    {
        OnGround = AddFeature(nameof(OnGround), "on ground")
            .AddLineStyleResource(nameof(LineStyle), LineStyle, "line style")
            .AddFloatResource(nameof(WorldCircleSize), "radius", 0, 2, () => WorldCircleSize, v => WorldCircleSize = v)
            .AddFloatResource(nameof(WorldCircleStroke), "stroke", 0, 10, () => WorldCircleStroke, v => WorldCircleStroke = v);
        OnMap = AddFeature(nameof(OnMap), "on map")
            .AddLineStyleResource(nameof(MapLineStyle), MapLineStyle, "line style")
            .AddFloatResource(nameof(MapCircleSize), "map radius", 0, 20, () => MapCircleSize, v => MapCircleSize = v)
            .AddFloatResource(nameof(MapCircleStroke), "map stroke", 0, 10, () => MapCircleStroke, v => MapCircleStroke = v);
        Developer = AddFeature(nameof(Developer), "developer")
            .AddBooleanResource(nameof(OnMonsters), "`on monsters", () => OnMonsters, v => OnMonsters = v)
            .AddBooleanResource(nameof(OnGizmos), "`on gizmos", () => OnGizmos, v => OnGizmos = v)
            .AddBooleanResource(nameof(OnGenerics), "`on generics", () => OnGenerics, v => OnGenerics = v)
            .AddBooleanResource(nameof(OnGenerics), "`on items", () => OnItems, v => OnItems = v);
    }

    public void PaintGameWorld(GameWorldLayer layer)
    {
        if (Game.MyPlayer.LevelAreaSno.IsTown)
            return;

        switch (layer)
        {
            case GameWorldLayer.Ground:
                foreach (var gizmo in Game.GizmoActors.Where(x => SnoIdsSet.Contains(x.ActorSno.SnoId)))
                {
                    if (!gizmo.Coordinate.IsOnScreen)
                        continue;

                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, gizmo.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                foreach (var item in Game.Items.Where(x => x.Location == ItemLocation.None && SnoIdsSet.Contains(x.ActorSno.SnoId)))
                {
                    if (!item.Coordinate.IsOnScreen)
                        continue;

                    LineStyle.DrawWorldEllipse(WorldCircleSize, -1, item.Coordinate, strokeWidthCorrection: WorldCircleStroke);
                }

                if (!Developer.Enabled)
                    return;
                DrawDevActors(x => DebugActorSnoIdSet.Contains(x.ActorSno.SnoId), OnMonsters, OnGizmos, OnGenerics, OnItems);
                break;

            case GameWorldLayer.Map when Map.CurrentMode == MapMode.Minimap:
                foreach (var gizmo in Game.GizmoActors.Where(x => SnoIdsSet.Contains(x.ActorSno.SnoId)))
                {
                    if (!gizmo.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle.DrawEllipse(gizmo.Coordinate.MapX, gizmo.Coordinate.MapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                foreach (var item in Game.Items.Where(x => x.Location == ItemLocation.None && SnoIdsSet.Contains(x.ActorSno.SnoId)))
                {
                    if (!item.Coordinate.IsOnMap)
                        continue;

                    MapLineStyle.DrawEllipse(item.Coordinate.MapX, item.Coordinate.MapY, MapCircleSize, MapCircleSize, strokeWidthCorrection: MapCircleStroke);
                }

                break;
        }
    }

    private HashSet<ActorSnoId> SnoIdsSet { get; } = new()
    {
        ActorSnoId.DGN_Standard_Sitting_Skeleton_Switch,
        ActorSnoId.DGN_Standard_Global_Human_Stake_01_Switch_Dyn,
        ActorSnoId.DSH_Holdout_Switch,
        ActorSnoId.DSH_Spawner_Switch,
        // switch traps 1f
        ActorSnoId.DSH_Holdout_Switch_Spikes,
        ActorSnoId.DSH_Spawner_Switch_Spikes,
        // level floor switch
        ActorSnoId.DGN_Frac_KorDraganBarracks_FloorLever,
        // ActorSnoId.DGN_Frac_BlackAsylum_ChainReleaseWinch, // cause issues
        // ActorSnoId.DGN_Kehj_TomboftheSaints_ChainReleaseWinch, // cause issues
        ActorSnoId.Carryable_AncientsStatue,
        ActorSnoId.Carryable_Bloodstone,
        ActorSnoId.Carryable_CrusaderSkull,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_01,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_02,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Cursed_03,
        ActorSnoId.Carryable_HolyRelic_QST_Frac_Glacier_Purified,
        ActorSnoId.Carryable_Mechanical,
        ActorSnoId.Carryable_RunicStandingStone,
        ActorSnoId.Carryable_SightlessEye,
        ActorSnoId.Carryable_StoneCarving,
        ActorSnoId.Carryable_Winch,
        // keys
        ActorSnoId.Global_Flippy_Items_RustedIronKeys_01_Item,
        ActorSnoId.Global_Flippy_Items_RustedIronKeys_02_Item,
        ActorSnoId.Global_Flippy_Items_RustedIronKeys_03_Item,
        // defaced shrine
        ActorSnoId.Carryable_DefacedShrine,
        ActorSnoId.Receptacle_DefacedShrine,
    };

    private HashSet<ActorSnoId> DebugActorSnoIdSet { get; } = new()
    {
        ActorSnoId.Ancients_Prop_Lilith_BloodPetal_switch_smokeTrail_emitter,
        ActorSnoId.Bounty_RitualNode_Switch,
        ActorSnoId.CAMP_Frac_Nostrava_Locked_Door_Switch,
        ActorSnoId.CAMP_Frac_Nostrava_Locked_Door_Switch_Inn,
        ActorSnoId.CAMP_Frac_Nostrava_Prisoner_Switch,
        ActorSnoId.Camp_Waypoint_Claim_Switch,
        ActorSnoId.CSD_OldMines_HoleSwitch,
        ActorSnoId.DGN_Scos_TwistedHollow_BossEncounterStartSwitch,
        ActorSnoId.Generic_Switch_Plunger_01_Dyn,
        ActorSnoId.Global_Human_Stake_01_Switch_Dyn,
        ActorSnoId.Hawe_SnakeCultist_SnakeEyeSwitch_Dyn,
        ActorSnoId.HaweHU_Herbs_Incense_Switch_01_Dyn,
        ActorSnoId.HawezarHU_RottingShipTrapDoor_01_Switch_Dyn,
        ActorSnoId.HawezarHU_RottingShipTrapDoor_02_Switch_Dyn,
        ActorSnoId.Hell_Thematic_Event_Switch_01,
        ActorSnoId.Hell_Thematic_Event_Switch_02,
        ActorSnoId.Hell_Thematic_Event_Switch_03,
        ActorSnoId.LE_JarOfSouls_Switch,
        ActorSnoId.QST_Frac_Brazier_Switch,
        ActorSnoId.QST_Frac_Malady_ChaliceSwitch_01,
        ActorSnoId.QST_Frac_Malady_ChaliceSwitch_02,
        ActorSnoId.QST_Frac_Malady_ChaliceSwitch_03,
        ActorSnoId.QST_Frac_UnFlesh_Dagger_Switch,
        ActorSnoId.QST_Hawe_BadBlood_Camp_switch,
        ActorSnoId.QST_Hawe_BadBlood_Campfire_switch,
        ActorSnoId.QST_Hawe_BadBlood_Campfire_Switch_Corner_2x2_01_Fused,
        ActorSnoId.QST_Hawe_BadBlood_Campfire02_switch,
        ActorSnoId.QST_Hawe_Lyndon_03_Hide_switch,
        ActorSnoId.QST_Hawe_Marsh_MohlonIncenseBoxSwitch,
        ActorSnoId.QST_Hawe_Soulrot_AnimalCorpse_Switch,
        ActorSnoId.QST_Hawe_TheHeretic_AshNPCSwitch_00,
        ActorSnoId.QST_Hawe_TheHeretic_AshNPCSwitch_01,
        ActorSnoId.QST_Hawe_TheHeretic_AshNPCSwitch_02,
        ActorSnoId.QST_Hawe_Verge6_Chemistry_Switch_01,
        ActorSnoId.QST_Hawe_Verge6_Chemistry_Switch_02,
        ActorSnoId.QST_Hawe_Verge6_Chemistry_Switch_04,
        ActorSnoId.QST_Hawe_Verge6_Chemistry_Switch_05,
        ActorSnoId.QST_Hawe_ZakFort_SealedQuarters_RitualSwitch,
        ActorSnoId.QST_Kehj_Infection_01_FlyBodySwitch,
        ActorSnoId.QST_Kehj_Pilgrimage_PHWellSwitch,
        ActorSnoId.QST_Scos_ASoddenPact_WightStone_QuestSwitch,
        ActorSnoId.QST_Scos_Hills_VotivePassing_Torc_Switch,
        ActorSnoId.QST_Scos_Moors_UntamedThicket_NafainStone_Switch,
        ActorSnoId.QST_Scos_TheDiviner_DudSwitch01,
        ActorSnoId.QST_Scos_UnholyBloodlines_PotionEmpty_Switch,
        ActorSnoId.QST_Step_FiendSoFamiliar_Demonic_Cursed_Relic_Dyn_QuestSwitch,
        ActorSnoId.QST_Step_FiendSoFamiliar_Demonic_Cursed_Relic_Dyn_Switch,
        ActorSnoId.QST_Step_FiendSoFamiliar_Gizmo_LightSwitch,
        ActorSnoId.QST_Step_FiendSoFamiliar_Gizmo_LightSwitch03,
        ActorSnoId.QST_Step_SentimentalValue_Switch_Crumble,
        ActorSnoId.QST_Step_SentimentalValue_Switch_Jars,
        ActorSnoId.QST_Step_SentimentalValue_Switch_Rubble,
        ActorSnoId.QST_Step_WhatRemains_dirt_questswitch,
        ActorSnoId.QST_Template_Gizmo_QuestSwitch_Prefab_BeforeAfter,
        ActorSnoId.QST_Template_Gizmo_Switch_Blockout,
        ActorSnoId.QST_Template_Gizmo_Switch_Repeatable,
        ActorSnoId.SMP_Bone_Gizmo_Switch,
        ActorSnoId.SMP_Bury_Gizmo_Switch,
        ActorSnoId.SQ_Gizmo_Switch_Gather_Ichor,
        ActorSnoId.Switch_Scos_LairOfTheDespoiler_KeyStone,
        ActorSnoId.Switch_Scos_LairOfTheDespoiler_KeyStone02,
        ActorSnoId.Switch_Scos_LairOfTheDespoiler_KeyStone03,
        ActorSnoId.Template_Gizmo_QuestSwitch_Deliver,
    };
}