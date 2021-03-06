//Thank you Helldragger!

using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ColonistsDeco
{
    class JoyGiver_HangingWallDecoration : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            Thing wall = new Thing();
            Map pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner || pawn.ownership.OwnedBed == null || !pawn.TryGetComp<CompPawnDeco>().CanDecorate || pawn.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Ascetic")))
            {
                return null;
            }

            pawn.TryGetComp<CompPawnDeco>().ResetDecoCooldown();

            IList<IntVec3> wallLocations = pawn.ownership.OwnedBed.GetRoom().BorderCells.ToList();

            IList<Thing> wallThingList = new List<Thing>();

            foreach (IntVec3 wallLocation in wallLocations)
            {
                if (wallLocation.IsValid && wallLocation.InBounds(pawnMap))
                {
                    IList<Thing> wallTempThingList = wallLocation.GetThingList(pawnMap);
                    foreach (Thing wallThing in wallTempThingList)
                    {
                        if (Utility.IsWall(wallThing))
                        {
                            wallThingList.Add(wallThing);
                        }
                    }
                }
            }

            IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

            int wallDecoAmount = 0;

            foreach (Thing thingInRoom in thingsInRoom)
            {
                if(Utility.IsWallDeco(thingInRoom))
                {
                    wallDecoAmount++;
                }
            }

            wall = wallThingList.RandomElement();
            IntVec3 randomPlacePos = IntVec3.Invalid;
            int i = 0;
            while (i < 4)
            {
                IntVec3 intVec = wall.Position + GenAdj.CardinalDirections[i];
                Region region = (wall.Position + GenAdj.CardinalDirections[i]).GetRegion(pawnMap);
                if (region != null && region.Room == pawn.ownership.OwnedBed.GetRoom())
                {
                    randomPlacePos = intVec;
                }
                int num = i + 1;
                i = num;
            }

            if (pawn.CanReserveAndReach(randomPlacePos, PathEndMode.OnCell, Danger.None, 1, -1) && pawn.CanReserve(wall, 1, -1, null, false) && !randomPlacePos.IsValid || wall == null || wall.def == new ThingDef() || wallDecoAmount >= ColonistsDecoMain.Settings.wallDecorationLimit)
            {
                return null;
            }

            Job job = JobMaker.MakeJob(def.jobDef, randomPlacePos, wall);
            return job;
        }
    }
}