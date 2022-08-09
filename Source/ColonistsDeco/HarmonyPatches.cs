using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ColonistsDeco
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type PatchType = typeof(HarmonyPatches);
        
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony(id: "rimworld.iemanddieabeheet.colonistsdeco");

            harmony.Patch(AccessTools.Method(typeof(BeautyUtility), nameof(BeautyUtility.CellBeauty)),
                postfix: new HarmonyMethod(PatchType, nameof(CellBeautyPostfix)));
        }

        public static void CellBeautyPostfix(ref float result, IntVec3 c, Map map)
        {
            List<IntVec3> cells = GenAdjFast.AdjacentCellsCardinal(c);
            foreach (IntVec3 cell in cells)
            {
                foreach(Thing thing in cell.GetThingList(map))
                {
                    if(Utility.wallDecoDefs.Contains(thing.def) && (thing.Position + thing.Rotation.FacingCell) == c)
                    {
                        result += thing.GetStatValue(StatDefOf.Beauty);
                    }
                }
            }
        }
    }
}
