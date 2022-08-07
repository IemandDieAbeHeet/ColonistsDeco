using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;
using RimWorld;

namespace Main
{
    public class JobDriver_HangingWallDecoration : JobDriver
    {
        private float workLeft = 100f;

        protected const int BaseWorkAmount = 100;
        protected LocalTargetInfo placeInfo => job.GetTarget(TargetIndex.A);
        protected LocalTargetInfo wallInfo => job.GetTarget(TargetIndex.B);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(placeInfo, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnBurningImmobile(TargetIndex.B);
            this.FailOnDestroyedOrNull(TargetIndex.B);

            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.B);

            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);

            Toil hangPoster = new Toil();
            hangPoster.handlingFacing = true;
            hangPoster.initAction = delegate
            {
                workLeft = 100f;
            };
            hangPoster.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(TargetB.Cell);
                if (pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Construction, 0.085f);
                }
                float statValue = pawn.GetStatValueForPawn(StatDefOf.ConstructionSpeed, pawn);
                workLeft -= statValue * 1.7f;
                if (workLeft <= 0f)
                {
                    Thing thing = new Thing();
                    List<ThingDef> wallDecos = Utility.GetDecoList(DecoLocationType.Wall);

                    IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

                    ThingDef wallDeco;

                    List<ThingDef> possibleWallDecos = new List<ThingDef>(wallDecos);
                    foreach (Thing thingInRoom in thingsInRoom)
                    {
                        if (Utility.IsCeilingDeco(thingInRoom))
                        {
                            possibleWallDecos.Remove(thingInRoom.def);
                        }
                    }

                    if (possibleWallDecos.Count > 0)
                    {
                        wallDeco = possibleWallDecos.RandomElement();
                    }
                    else
                    {
                        wallDeco = wallDecos.RandomElement();
                    }

                    thing = ThingMaker.MakeThing(wallDeco);

                    thing.SetFactionDirect(pawn.Faction);
                    CompDecoration compDecoration = thing.TryGetComp<CompDecoration>();
                    if(compDecoration != null)
                    {
                        compDecoration.decorationCreator = pawn.Name.ToStringShort;
                    }
                    wallInfo.Thing.TryGetComp<CompAttachableThing>().AddAttachment(thing);
                    GenSpawn.Spawn(thing, wallInfo.Cell, Map, pawn.Rotation.Opposite, WipeMode.Vanish, false);
                    ReadyForNextToil();
                }
            };
            hangPoster.defaultCompleteMode = ToilCompleteMode.Never;
            hangPoster.WithProgressBar(TargetIndex.A, () => (BaseWorkAmount - workLeft) / BaseWorkAmount, interpolateBetweenActorAndTarget: true);

            yield return hangPoster;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workLeft, "workLeft", 0f);
        }
    }
}
