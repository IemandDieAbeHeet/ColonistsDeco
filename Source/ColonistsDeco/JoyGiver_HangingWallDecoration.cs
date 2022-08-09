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
            var pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner ||
                pawn.ownership.OwnedBed == null || !pawn.TryGetComp<CompPawnDeco>().CanDecorate ||
                pawn.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Ascetic")))
            {
                return null;
            }

            pawn.TryGetComp<CompPawnDeco>().ResetDecoCooldown();

            IList<IntVec3> wallLocations = pawn.ownership.OwnedBed.GetRoom().BorderCells.ToList();

            IList<Thing> wallThingList = (from wallLocation in wallLocations
                where wallLocation.IsValid && wallLocation.InBounds(pawnMap)
                select wallLocation.GetThingList(pawnMap)
                into wallTempThingList
                from wallThing in wallTempThingList
                where Utility.IsWall(wallThing)
                select wallThing).ToList();

            IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

            var wallDecoAmount = thingsInRoom.Count(Utility.IsWallDeco);

            var wall = wallThingList.RandomElement();
            var randomPlacePos = IntVec3.Invalid;
            var i = 0;
            var intVec = wall.Position + GenAdj.CardinalDirections[i];
            while (i < 4)
            {
                var region = (wall.Position + GenAdj.CardinalDirections[i]).GetRegion(pawnMap);
                if (region != null && region.Room == pawn.ownership.OwnedBed.GetRoom())
                {
                    randomPlacePos = intVec;
                }
                var num = i + 1;
                i = num;
            }

            if (pawn.CanReserveAndReach(randomPlacePos, PathEndMode.OnCell, Danger.None) && pawn.CanReserve(wall) &&
                !randomPlacePos.IsValid || wall.def == new ThingDef() ||
                wallDecoAmount >= ColonistsDecoMain.settings.wallDecorationLimit)
            {
                return null;
            }

            var job = JobMaker.MakeJob(def.jobDef, randomPlacePos, wall);
            return job;
        }
    }
}