using Verse;
using RimWorld;
using UnityEngine;
using System.Linq; // Required for .Any() extension method

namespace ResistanceRestraintsMod
{
    public class CompReduceResistanceInBed : ThingComp
    {
        public CompProperties_ReduceResistanceInBed Props => (CompProperties_ReduceResistanceInBed)props;

        public override void CompTick()
        {
            base.CompTick();

            if (parent is Building_Bed bed && bed.CurOccupants.Any()) // Correct way to check if pawns are in the bed
            {
                foreach (Pawn pawn in bed.CurOccupants) // Correct way to get pawns in the bed
                {
                    if (pawn?.guest != null && pawn.IsPrisonerOfColony)
                    {
                        float reductionAmount = Props.baseReduction;

                        // Apply mod settings multiplier based on device type
                        string defName = parent.def.defName;
                        if (ModSettings_ResistanceRestraints.Settings != null)
                        {
                            if (defName == "SilkCircuit_TortureBed")
                            {
                                reductionAmount *= ModSettings_ResistanceRestraints.Settings.restrainingTableMultiplier;
                            }
                            else if (defName == "SilkCircuit_PrisonerCage")
                            {
                                reductionAmount *= ModSettings_ResistanceRestraints.Settings.humiliationCageMultiplier;
                            }
                            else if (defName == "SilkCircuit_ChemfuelBath")
                            {
                                reductionAmount *= ModSettings_ResistanceRestraints.Settings.chemfuelBathMultiplier;
                            }
                            // Sensory collapser uses instant zero, so no multiplier needed
                        }

                        // Additional reduction if pawn is immobilized
                        if (pawn.health.hediffSet.HasHediff(HediffDef.Named("SilkCircuit_Immobile")))
                        {
                            reductionAmount *= Props.immobileMultiplier;
                        }

                        // Get current values
                        float currentResistance = Mathf.Max(pawn.guest.resistance, 0f);
                        float currentWill = 0f;
                        bool hasWill = false;
                        
                        // Check if Ideology is active and pawn has will (for slavery)
                        if (ModsConfig.IdeologyActive && pawn.guest != null)
                        {
                            currentWill = Mathf.Max(pawn.guest.will, 0f);
                            hasWill = currentWill > 0f;
                        }

                        // Calculate proportional reduction so both reach 0 at the same time
                        float totalToReduce = currentResistance + currentWill;
                        
                        if (totalToReduce > 0f)
                        {
                            // Calculate proportional reductions
                            float resistancePortion = currentResistance / totalToReduce;
                            float willPortion = currentWill / totalToReduce;
                            
                            // Apply resistance reduction (ensuring it never goes below 0)
                            float resistanceReduction = reductionAmount * resistancePortion;
                            pawn.guest.resistance = Mathf.Max(currentResistance - resistanceReduction, 0f);
                            
                            // Apply will reduction if Ideology is active and pawn has will
                            if (hasWill && ModsConfig.IdeologyActive)
                            {
                                float willReduction = reductionAmount * willPortion;
                                pawn.guest.will = Mathf.Max(currentWill - willReduction, 0f);
                            }
                        }
                        // If both are already at 0, do nothing (no reduction needed)
                    }
                }
            }
        }
    }

    public class CompProperties_ReduceResistanceInBed : CompProperties
    {
        public float baseReduction = 0.05f; // Default resistance reduction per tick
        public float immobileMultiplier = 2f; // Multiplier if the pawn is immobilized

        public CompProperties_ReduceResistanceInBed()
        {
            compClass = typeof(CompReduceResistanceInBed);
        }
    }
}
