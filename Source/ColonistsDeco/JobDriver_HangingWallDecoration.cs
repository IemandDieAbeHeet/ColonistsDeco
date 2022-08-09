using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ColonistsDeco
{
    public class JobDriver_HangingWallDecoration : JobDriver
    {
        private float _workLeft = 100f;

        private const int BaseWorkAmount = 100;
        private LocalTargetInfo PlaceInfo => job.GetTarget(TargetIndex.A);
        private LocalTargetInfo WallInfo => job.GetTarget(TargetIndex.B);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(PlaceInfo, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            this.FailOnBurningImmobile(TargetIndex.B);
            this.FailOnDestroyedOrNull(TargetIndex.B);

            yield return Toils_Reserve.Reserve(TargetIndex.A);
            yield return Toils_Reserve.Reserve(TargetIndex.B);

            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);

            Toil hangPoster = new Toil
            {
                handlingFacing = true,
                initAction = delegate
                {
                    _workLeft = 100f;
                },
                tickAction = delegate
                {
                    pawn.rotationTracker.FaceCell(TargetB.Cell);
                    pawn.skills?.Learn(SkillDefOf.Construction, 0.085f);
                    
                    var statValue = pawn.GetStatValueForPawn(StatDefOf.ConstructionSpeed, pawn);
                    _workLeft -= statValue * 1.7f;
                    var wallDecos = Utility.GetDecoList(DecoLocationType.Wall);
                    
                    if (!(_workLeft <= 0f)) return;
                    IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

                    var possibleWallDecos = new List<ThingDef>(wallDecos);
                    foreach (var thingInRoom in thingsInRoom)
                    {
                        if (Utility.IsCeilingDeco(thingInRoom))
                        {
                            possibleWallDecos.Remove(thingInRoom.def);
                        }
                    }

                    var wallDeco = possibleWallDecos.Count > 0 ? possibleWallDecos.RandomElement() : wallDecos.RandomElement();

                    var thing = ThingMaker.MakeThing(wallDeco);

                    thing.SetFactionDirect(pawn.Faction);
                    var compDecoration = thing.TryGetComp<CompDecoration>();
                    if(compDecoration != null)
                    {
                        compDecoration.decorationCreator = pawn.Name.ToStringShort;
                    }
                    WallInfo.Thing.TryGetComp<CompAttachableThing>().AddAttachment(thing);
                    GenSpawn.Spawn(thing, WallInfo.Cell, Map, pawn.Rotation.Opposite);
                    ReadyForNextToil();
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };
            hangPoster.WithProgressBar(TargetIndex.A, () => (BaseWorkAmount - _workLeft) / BaseWorkAmount, interpolateBetweenActorAndTarget: true);

            yield return hangPoster;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _workLeft, "workLeft", 0f);
        }
    }
}
