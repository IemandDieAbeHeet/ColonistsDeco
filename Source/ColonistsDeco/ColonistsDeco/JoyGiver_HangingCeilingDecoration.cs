using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using RimWorld;

namespace ColonistsDeco
{
    class JoyGiver_HangingCeilingDecoration : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            IntVec3 ceilingLocation;
            Map pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner || pawn.ownership.OwnedBed == null || !pawn.TryGetComp<CompPawnDeco>().CanDecorate)
            {
                return null;
            }

            pawn.TryGetComp<CompPawnDeco>().ResetDecoCooldown();

            IEnumerable<IntVec3> tempCeilingLocations = pawn.ownership.OwnedBed.GetRoom().Cells;
            IList<IntVec3> ceilingLocations = new List<IntVec3>();

            foreach (IntVec3 tempCeilingLocation in tempCeilingLocations)
            {
                if (tempCeilingLocation.IsValid && tempCeilingLocation.InBounds(pawnMap) && !tempCeilingLocation.Filled(pawnMap) && tempCeilingLocation.Roofed(pawnMap))
                {
                    ceilingLocations.Add(tempCeilingLocation);
                }
            }

            if (ceilingLocations.Count > 0)
            {
                ceilingLocation = ceilingLocations.RandomElement();
            } else
            {
                return null;
            }

            IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

            int ceilingDecorationAmount = 0;

            foreach (Thing thingInRoom in thingsInRoom)
            {
                if (Utility.IsCeilingDeco(thingInRoom))
                {
                    ceilingDecorationAmount++;
                }
            }

            if (ceilingLocation == null || ceilingDecorationAmount >= ColonistsDecoMain.Settings.ceilingDecorationLimit)
            {
                return null;
            }

            Job job = JobMaker.MakeJob(def.jobDef, ceilingLocation);
            return job;
        }
    }
}
