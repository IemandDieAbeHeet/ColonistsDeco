using Verse;
using RimWorld;
using System;
using HarmonyLib;
using System.Collections.Generic;

namespace ColonistsDeco
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);
        
        static HarmonyPatches()
        {
            Harmony harmony = new Harmony(id: "rimworld.iemanddieabeheet.colonistsdeco");

            harmony.Patch(AccessTools.Method(typeof(BeautyUtility), nameof(BeautyUtility.CellBeauty)),
                postfix: new HarmonyMethod(patchType, nameof(CellBeautyPostfix)));
        }

        public static void CellBeautyPostfix(ref float __result, IntVec3 c, Map map)
        {
            List<IntVec3> cells = GenAdjFast.AdjacentCellsCardinal(c);
            foreach (IntVec3 cell in cells)
            {
                foreach(Thing thing in cell.GetThingList(map))
                {
                    if(Utility.wallDefs.Contains(thing.def) && (thing.Position + thing.Rotation.FacingCell) == c)
                    {
                        __result += thing.GetStatValue(StatDefOf.Beauty);
                    }
                }
            }
        }
    }
}
