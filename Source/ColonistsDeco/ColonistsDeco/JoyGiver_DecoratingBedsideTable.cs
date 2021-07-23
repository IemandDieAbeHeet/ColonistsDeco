using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld;
using Verse.AI;

namespace ColonistsDeco
{
    class JoyGiver_DecoratingBedsideTable : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            Thing bedsideTable;
            Map pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner || pawn.ownership.OwnedBed == null)
            {
                return null;
            }

            IList<Thing> bedsideTables = pawn.ownership.OwnedBed.GetRoom().ContainedThings(DefDatabase<ThingDef>.GetNamed("EndTable")).ToList();

            for(int i = bedsideTables.Count - 1; i >= 0; i--)
            {
                IList <Thing> ThingList = bedsideTables[i].Position.GetThingList(pawnMap);
                if (ThingList.Any(b => Utility.IsBedsideDeco(b)))
                {
                    bedsideTables.RemoveAt(i);
                }
            }

            if (bedsideTables.NullOrEmpty())
            {
                return null;
            }

            bedsideTable = bedsideTables.RandomElement();

            Thing emptyThing = new Thing();
            if (bedsideTable == null || bedsideTable.def == emptyThing.def)
            {
                return null;
            }

            Job job = JobMaker.MakeJob(def.jobDef, bedsideTable);
            return job;
        }
    }
}