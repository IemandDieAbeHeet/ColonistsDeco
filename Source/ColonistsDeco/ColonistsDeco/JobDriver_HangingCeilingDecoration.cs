using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;

namespace ColonistsDeco
{
    class JobDriver_HangingCeilingDecoration : JobDriver
    {
        private float workLeft = 100f;

        protected const int BaseWorkAmount = 100;
        protected LocalTargetInfo placeInfo => job.GetTarget(TargetIndex.A);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(placeInfo, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Reserve.Reserve(TargetIndex.A);

            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);

            Toil hangCeilingDeco = new Toil();

            hangCeilingDeco.handlingFacing = true;
            hangCeilingDeco.initAction = delegate
            {
                workLeft = 100f;
            };
            hangCeilingDeco.tickAction = delegate
            {
                if (pawn.skills != null)
                {
                    pawn.skills.Learn(SkillDefOf.Construction, 0.085f);
                }
                float statValue = pawn.GetStatValueForPawn(StatDefOf.ConstructionSpeed, pawn);
                workLeft -= statValue * 1.7f;
                if (workLeft <= 0f)
                {
                    Thing thing = new Thing();
                    thing = ThingMaker.MakeThing(Utility.ceilingDefs.RandomElement());
                    thing.SetFactionDirect(pawn.Faction);
                    GenSpawn.Spawn(thing, placeInfo.Cell, Map, Rot4.North, WipeMode.Vanish, false);
                    ReadyForNextToil();
                }
            };
            hangCeilingDeco.defaultCompleteMode = ToilCompleteMode.Never;
            hangCeilingDeco.WithProgressBar(TargetIndex.A, () => (BaseWorkAmount - workLeft) / BaseWorkAmount, interpolateBetweenActorAndTarget: true);

            yield return hangCeilingDeco;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref workLeft, "workLeft", 0f);
        }
    }
}
