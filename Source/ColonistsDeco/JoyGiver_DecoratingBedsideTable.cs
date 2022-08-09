using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace ColonistsDeco
{
    class JoyGiver_DecoratingBedsideTable : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            var pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner || pawn.ownership.OwnedBed == null)
            {
                return null;
            }

            IList<Thing> bedsideTables = pawn.ownership.OwnedBed.GetRoom().ContainedThings(DefDatabase<ThingDef>.GetNamed("EndTable")).ToList();

            for(var i = bedsideTables.Count - 1; i >= 0; i--)
            {
                IList<Thing> thingList = bedsideTables[i].Position.GetThingList(pawnMap);
                if (thingList.Any(Utility.IsBedsideDeco))
                {
                    bedsideTables.RemoveAt(i);
                }
            }

            if (bedsideTables.NullOrEmpty())
            {
                return null;
            }

            var bedsideTable = bedsideTables.RandomElement();

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