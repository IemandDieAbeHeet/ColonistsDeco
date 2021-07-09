using System;
using System.Collections.Generic;
using System.Diagnostics;
using Verse;
using Verse.AI;
using RimWorld;

namespace ColonistsDeco
{
    public class JobDriver_DecoratingBedsideTable : JobDriver
    {
        private float workLeft = 100f;

        protected const int BaseWorkAmount = 100;
        protected LocalTargetInfo bedsideInfo => job.GetTarget(TargetIndex.A);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(bedsideInfo, job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnBurningImmobile(TargetIndex.A);
            this.FailOnDestroyedOrNull(TargetIndex.A);

            yield return Toils_Reserve.Reserve(TargetIndex.A);

            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);

            Toil decorateBedside = new Toil();
            decorateBedside.handlingFacing = true;
            decorateBedside.initAction = delegate
            {
                workLeft = 100f;
            };
            decorateBedside.tickAction = delegate
            {
                pawn.rotationTracker.FaceCell(TargetA.Cell);
                if (pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Construction, 0.085f);
                }
                float statValue = pawn.GetStatValueForPawn(StatDefOf.ConstructionSpeed, pawn);
                workLeft -= statValue * 1.7f;
                if (workLeft <= 0f)
                {
                    Thing thing = ThingMaker.MakeThing(Utility.bedsideDefs.RandomElement());
                    thing.SetFactionDirect(pawn.Faction);
                    bedsideInfo.Thing.TryGetComp<CompAttachableThing>().AddAttachment(thing);
                    GenSpawn.Spawn(thing, bedsideInfo.Cell, Map, bedsideInfo.Thing.Rotation, WipeMode.Vanish, false);
                    ReadyForNextToil();
                }
            };
            decorateBedside.defaultCompleteMode = ToilCompleteMode.Never;
            decorateBedside.WithProgressBar(TargetIndex.A, () => (BaseWorkAmount - workLeft) / BaseWorkAmount, interpolateBetweenActorAndTarget: true);

            yield return decorateBedside;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workLeft, "workLeft", 0f);
        }
    }
}