using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ColonistsDeco
{
    public class JobDriver_DecoratingBedsideTable : JobDriver
    {
        private float _workLeft = 100f;

        private const int BaseWorkAmount = 100;
        private LocalTargetInfo BedsideInfo => job.GetTarget(TargetIndex.A);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(BedsideInfo, job);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnBurningImmobile(TargetIndex.A);
            this.FailOnDestroyedOrNull(TargetIndex.A);

            yield return Toils_Reserve.Reserve(TargetIndex.A);

            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.Touch);

            var decorateBedside = new Toil
            {
                handlingFacing = true,
                initAction = delegate
                {
                    _workLeft = 100f;
                },
                tickAction = delegate
                {
                    pawn.rotationTracker.FaceCell(TargetA.Cell);
                    pawn.skills?.Learn(SkillDefOf.Construction, 0.085f);
                    var statValue = pawn.GetStatValueForPawn(StatDefOf.ConstructionSpeed, pawn);
                    _workLeft -= statValue * 1.7f;
                    if (!(_workLeft <= 0f)) return;
                    var thing = ThingMaker.MakeThing(Utility.bedsideDecoDefs.RandomElement());
                    thing.SetFactionDirect(pawn.Faction);
                    BedsideInfo.Thing.TryGetComp<CompAttachableThing>().AddAttachment(thing);
                    GenSpawn.Spawn(thing, BedsideInfo.Cell, Map, BedsideInfo.Thing.Rotation, WipeMode.Vanish, false);
                    ReadyForNextToil();
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
            decorateBedside.WithProgressBar(TargetIndex.A, () => (BaseWorkAmount - _workLeft) / BaseWorkAmount, interpolateBetweenActorAndTarget: true);

            yield return decorateBedside;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _workLeft, "workLeft", 0f);
        }
    }
}