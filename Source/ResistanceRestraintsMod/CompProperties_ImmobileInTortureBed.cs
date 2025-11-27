using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;

namespace ResistanceRestraintsMod
{
    public class CompProperties_TortureBed : CompProperties
    {
        public CompProperties_TortureBed()
        {
            this.compClass = typeof(CompTortureBed);
        }
    }

    public class CompTortureBed : ThingComp
    {
        private Building_Bed bed => this.parent as Building_Bed;
        private CompRefuelable compRefuelable => this.parent.GetComp<CompRefuelable>();
        private CompPowerTrader compPowerTrader => this.parent.GetComp<CompPowerTrader>();
        private HashSet<Pawn> affectedPawns = new HashSet<Pawn>();
        
        // Static set to track pawns that are being released (to prevent hediff reapplication)
        public static HashSet<Pawn> pawnsBeingReleased = new HashSet<Pawn>();
        
        // Static dictionary to track which bed each pawn was released from (to prevent re-trapping)
        public static Dictionary<Pawn, Building_Bed> pawnsReleasedFromBed = new Dictionary<Pawn, Building_Bed>();

        public override void CompTick()
        {
            base.CompTick();
            if (bed == null) return;

            // Check if the building has fuel (if applicable)
            bool hasFuel = compRefuelable?.HasFuel ?? true; // Assume true if no refuelable component

            // Check if the building has power (if applicable)
            bool hasPower = compPowerTrader?.PowerOn ?? true; // Assume true if no power component

            // The bed must have both fuel and power to apply the hediff
            bool canApplyHediff = hasFuel && hasPower;

            List<Pawn> currentOccupants = bed.CurOccupants?.ToList() ?? new List<Pawn>();

            foreach (Pawn pawn in currentOccupants)
            {
                if (pawn == null || pawn.Dead || pawn.Downed) continue;

                // Skip if this pawn is being released
                if (pawnsBeingReleased.Contains(pawn))
                {
                    continue;
                }

                // Safety check: If this pawn was released from ANY restraint bed, don't apply hediff and unassign them
                // This prevents re-trapping in any restraint bed (Restraining Table, Humiliation Cage, Chemfuel Bath, Sensory Collapser)
                if (pawnsReleasedFromBed.ContainsKey(pawn))
                {
                    // Unassign the bed from this pawn to prevent them from coming back
                    CompAssignableToPawn_Bed assignableComp = bed.GetComp<CompAssignableToPawn_Bed>();
                    if (assignableComp != null && assignableComp.AssignedPawns.Contains(pawn))
                    {
                        assignableComp.TryUnassignPawn(pawn);
                    }
                    continue; // Skip applying hediff to prevent re-trapping
                }

                Hediff immobilityHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SilkCircuit.SilkCircuit_Immobile);

                if (canApplyHediff)
                {
                    // Add the hediff if not already present
                    if (immobilityHediff == null)
                    {
                        pawn.health.AddHediff(HediffDefOf_SilkCircuit.SilkCircuit_Immobile);
                        affectedPawns.Add(pawn);
                    }
                }
                else
                {
                    // Remove the hediff if the bed is out of fuel or power
                    if (immobilityHediff != null)
                    {
                        pawn.health.RemoveHediff(immobilityHediff);
                        affectedPawns.Remove(pawn);
                    }
                }
            }

            // Remove the hediff from any previously affected pawns who are no longer in bed or if the bed is out of fuel/power
            foreach (Pawn pawn in affectedPawns.ToList()) // Copy to avoid modifying collection during iteration
            {
                if (!currentOccupants.Contains(pawn) || !canApplyHediff)
                {
                    Hediff immobilityHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SilkCircuit.SilkCircuit_Immobile);
                    if (immobilityHediff != null)
                    {
                        pawn.health.RemoveHediff(immobilityHediff);
                    }
                    affectedPawns.Remove(pawn);
                    // Also clean up release tracking if pawn left the bed
                    pawnsBeingReleased.Remove(pawn);
                }
            }
            
            // Clean up release tracking for pawns no longer in bed
            pawnsBeingReleased.RemoveWhere(pawn => !currentOccupants.Contains(pawn));
            
            // Clean up bed release tracking for pawns no longer in any restraint bed
            List<Pawn> pawnsToRemove = new List<Pawn>();
            foreach (var kvp in pawnsReleasedFromBed)
            {
                if (kvp.Key == null || kvp.Key.Dead || !kvp.Key.Spawned)
                {
                    pawnsToRemove.Add(kvp.Key);
                }
                else
                {
                    // If pawn is not in any restraint bed, clear the tracking
                    Building_Bed currentBed = kvp.Key.CurrentBed() as Building_Bed;
                    if (currentBed == null || !IsRestraintBed(currentBed))
                    {
                        pawnsToRemove.Add(kvp.Key);
                    }
                }
            }
            foreach (Pawn pawn in pawnsToRemove)
            {
                pawnsReleasedFromBed.Remove(pawn);
            }
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);

            // Ensure any pawn still tracked gets the hediff removed
            foreach (Pawn pawn in affectedPawns)
            {
                Hediff immobilityHediff = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf_SilkCircuit.SilkCircuit_Immobile);
                if (immobilityHediff != null)
                {
                    pawn.health.RemoveHediff(immobilityHediff);
                }
            }

            affectedPawns.Clear();
            
            // Clean up release tracking for this specific bed (in case it was destroyed)
            // Remove any pawns that were released from this bed
            List<Pawn> pawnsToRemove = new List<Pawn>();
            foreach (var kvp in pawnsReleasedFromBed)
            {
                if (kvp.Value == bed)
                {
                    pawnsToRemove.Add(kvp.Key);
                }
            }
            foreach (Pawn pawn in pawnsToRemove)
            {
                pawnsReleasedFromBed.Remove(pawn);
            }
        }
        
        // Helper method to check if a bed is a restraint bed
        private bool IsRestraintBed(Building_Bed bed)
        {
            if (bed == null || bed.def == null) return false;
            return bed.def.thingCategories != null && 
                   bed.def.thingCategories.Contains(ThingCategoryDef.Named("BuildingsPrisonerRestraints"));
        }
    }

    [DefOf]
    public static class HediffDefOf_SilkCircuit
    {
        public static HediffDef SilkCircuit_Immobile;

        static HediffDefOf_SilkCircuit()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(HediffDefOf_SilkCircuit));
        }
    }
}
