namespace T4.Plugins.Troubadour;

public static class DungeonQuestsStore
{
    public static IEnumerable<ActorSnoId> ActorSnoIds { get; } = new List<ActorSnoId>
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
        // defaced shrine
        ActorSnoId.Receptacle_DefacedShrine,

    };

    public static IEnumerable<ActorSnoId> DebugActorSnoIds { get; } = new List<ActorSnoId>
    {
        // ActorSnoId.Symbol_Quest_Proxy, // probably not needed/wanted 
        ActorSnoId.Ancients_Prop_Lilith_BloodPetal_switch_smokeTrail_emitter,
        ActorSnoId.Bounty_RitualNode_Switch,
        ActorSnoId.CAMP_Frac_Nostrava_Locked_Door_Switch,
        ActorSnoId.CAMP_Frac_Nostrava_Locked_Door_Switch_Inn,
        ActorSnoId.CAMP_Frac_Nostrava_Prisoner_Switch,
        ActorSnoId.Camp_Waypoint_Claim_Switch,
        ActorSnoId.CSD_OldMines_HoleSwitch,
        ActorSnoId.DGN_Scos_TwistedHollow_BossEncounterStartSwitch,
        // ActorSnoId.DONOTUSE__Template_Gizmo_QuestSwitch,
        // ActorSnoId.DONOTUSE__Template_Gizmo_QuestSwitch_InjectItem,
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

    public static HashSet<ActorSnoId> ActorSnoIdSet { get; } = new(ActorSnoIds);
    public static HashSet<ActorSnoId> DebugActorSnoIdSet { get; } = new(DebugActorSnoIds);
}