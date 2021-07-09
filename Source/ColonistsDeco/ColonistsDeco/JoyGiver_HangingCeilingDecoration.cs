using System.Threading.Tasks;
using Verse;
using Verse.AI;
using RimWorld;

namespace ColonistsDeco
{
    class JoyGiver_HangingCeilingDecoration : JoyGiver
    {
        public override Job TryGiveJob(Pawn pawn)
        {
            Thing wall = new Thing();
            Map pawnMap = pawn.Map;

            if (pawn.WorkTypeIsDisabled(WorkTypeDefOf.Construction) || pawn.IsPrisoner || pawn.ownership.OwnedBed == null || !pawn.TryGetComp<CompPawnDeco>().CanDecorate)
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
                    if (wallTempThingList.Any(w => Utility.IsPoster(w)))
                    {
                        continue;
                    }
                    else
                    {
                        foreach (Thing wallThing in wallTempThingList)
                        {
                            if (Utility.IsWall(wallThing))
                            {
                                wallThingList.Add(wallThing);
                            }
                        }
                    }
                }
            }

            IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

            int posterAmount = 0;

            foreach (Thing thingInRoom in thingsInRoom)
            {
                if (Utility.IsPoster(thingInRoom))
                {
                    posterAmount++;
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

            if (!randomPlacePos.IsValid || wall == null || wall.def == new ThingDef() || posterAmount >= ColonistsDecoMain.Settings.posterLimit)
            {
                return null;
            }

            Job job = JobMaker.MakeJob(def.jobDef, randomPlacePos, wall);
            return job;
        }
    }
}
