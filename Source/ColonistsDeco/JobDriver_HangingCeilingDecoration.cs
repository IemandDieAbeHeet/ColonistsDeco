using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace ColonistsDeco
{
    class JobDriver_HangingCeilingDecoration : JobDriver
    {
        private float _workLeft = 100f;

        private const int BaseWorkAmount = 100;
        private LocalTargetInfo PlaceInfo => job.GetTarget(TargetIndex.A);
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(PlaceInfo, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            yield return Toils_Reserve.Reserve(TargetIndex.A);

            yield return Toils_Goto.GotoCell(TargetIndex.A, PathEndMode.OnCell);

            var hangCeilingDeco = new Toil
            {
                handlingFacing = true,
                initAction = delegate
                {
                    _workLeft = 100f;
                },
                tickAction = delegate
                {
                    pawn.skills?.Learn(SkillDefOf.Construction, 0.085f);
                    var statValue = pawn.GetStatValueForPawn(StatDefOf.ConstructionSpeed, pawn);
                    _workLeft -= statValue * 1.7f;
                    if (!(_workLeft <= 0f)) return;
                    var ceilingDecos = Utility.GetDecoList(DecoLocationType.Ceiling);

                    IList<Thing> thingsInRoom = pawn.ownership.OwnedBed.GetRoom().ContainedAndAdjacentThings;

                    var possibleCeilingDecos = new List<ThingDef>(ceilingDecos);
                    foreach (var thingInRoom in thingsInRoom)
                    {
                        if(Utility.IsCeilingDeco(thingInRoom))
                        {
                            possibleCeilingDecos.Remove(thingInRoom.def);
                        }
                    }

                    var ceilingDeco = possibleCeilingDecos.Count >
                                      0 ? possibleCeilingDecos.RandomElement() : ceilingDecos.RandomElement();

                    var thing = ThingMaker.MakeThing(ceilingDeco);
                    thing.SetFactionDirect(pawn.Faction);
                    var compDecoration = thing.TryGetComp<CompDecoration>();
                    if (compDecoration != null)
                    {
                        compDecoration.decorationCreator = pawn.Name.ToStringShort;
                    }
                    GenSpawn.Spawn(thing, PlaceInfo.Cell, Map, Rot4.North, WipeMode.Vanish, false);
                    ReadyForNextToil();
                },
                defaultCompleteMode = ToilCompleteMode.Never
            };

            hangCeilingDeco.WithProgressBar(TargetIndex.A, () => (BaseWorkAmount - _workLeft) / BaseWorkAmount, interpolateBetweenActorAndTarget: true);

            yield return hangCeilingDeco;
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref _workLeft, "workLeft");
        }
    }
}
