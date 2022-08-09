using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ColonistsDeco
{
    class JoyGiver_HangingCeilingDecoration : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            IntVec3 ceilingLocation;
            var pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner ||
                pawn.ownership.OwnedBed == null || !pawn.TryGetComp<CompPawnDeco>().CanDecorate ||
                pawn.story.traits.HasTrait(DefDatabase<TraitDef>.GetNamed("Ascetic")))
            {
                return null;
            }

            pawn.TryGetComp<CompPawnDeco>().ResetDecoCooldown();

            var tempCeilingLocations = pawn.ownership.OwnedBed.GetRoom().Cells;
            IList<IntVec3> ceilingLocations = tempCeilingLocations.Where(tempCeilingLocation =>
                tempCeilingLocation.IsValid && tempCeilingLocation.InBounds(pawnMap) &&
                !tempCeilingLocation.Filled(pawnMap) && tempCeilingLocation.GetThingList(pawnMap).Count == 0 &&
                tempCeilingLocation.Roofed(pawnMap)).ToList();

            if (ceilingLocations.Count > 0)
            {
                ceilingLocation = ceilingLocations.RandomElement();
            } else
            {
                return null;
            }

            IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

            var ceilingDecorationAmount = thingsInRoom.Count(Utility.IsCeilingDeco);

            if (pawn.CanReserveAndReach(ceilingLocation, PathEndMode.OnCell, Danger.None) && ceilingLocation == null 
                || ceilingDecorationAmount >= ColonistsDecoMain.settings.ceilingDecorationLimit)
            {
                return null;
            }

            var job = JobMaker.MakeJob(def.jobDef, ceilingLocation);
            return job;
        }
    }
}
