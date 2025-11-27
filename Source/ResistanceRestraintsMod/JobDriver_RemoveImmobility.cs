using Verse;
using RimWorld;
using Verse.AI;
using System.Collections.Generic;

namespace ResistanceRestraintsMod
{
    public class JobDriver_RemoveImmobility : JobDriver
    {
        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(TargetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            Pawn targetPawn = TargetA.Pawn;
            
            // Ensure the job has a valid prisoner
            if (targetPawn == null)
            {
                yield break;
            }

            // PRE-MARK as being released IMMEDIATELY to prevent same-tick reapply race condition
            // This must happen before we even start moving, so CompTortureBed.CompTick() 
            // won't reapply the hediff while we're walking over
            CompTortureBed.pawnsBeingReleased.Add(targetPawn);

            // Move to the prisoner
            Toil goToPrisoner = Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.ClosestTouch);
            yield return goToPrisoner;

            // Perform the removal
            Toil removeHediff = new Toil();
            removeHediff.initAction = () =>
            {
                // Remove the immobility hediff
                Hediff immobilityHediff = targetPawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SilkCircuit.SilkCircuit_Immobile);
                if (immobilityHediff != null)
                {
                    targetPawn.health.RemoveHediff(immobilityHediff);
                }
                
                // Force the pawn to get up from bed by ending their current LayDown job
                if (targetPawn.CurJobDef == JobDefOf.LayDown && targetPawn.jobs.curJob != null)
                {
                    targetPawn.jobs.EndCurrentJob(JobCondition.InterruptForced);
                }
            };
            removeHediff.defaultCompleteMode = ToilCompleteMode.Instant;
            removeHediff.AddFinishAction(() =>
            {
                // Clean up tracking if job fails or is interrupted (hediff still present)
                // If hediff was successfully removed, keep them in tracking until they leave the bed
                // (CompTortureBed.CompTick() will clean it up automatically when they exit)
                if (targetPawn != null && targetPawn.health.hediffSet.HasHediff(HediffDefOf_SilkCircuit.SilkCircuit_Immobile))
                {
                    // Job failed - hediff still present, so remove from tracking
                    CompTortureBed.pawnsBeingReleased.Remove(targetPawn);
                }
            });
            yield return removeHediff;
        }
    }
}
