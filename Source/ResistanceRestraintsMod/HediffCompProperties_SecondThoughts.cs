using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using RimWorld;
using System;

namespace ResistanceRestraintsMod
{
    public class HediffCompProperties_SecondThoughts : HediffCompProperties
    {
        public bool removePrisonerStatus = true; // Default behavior
        public float resistanceReduction = 0f;   // Allows optional resistance reduction
        public bool instantRecruit = false;      // Option to instantly add them to the player's faction
        public float finalResistanceMin = 1f;    // Minimum final resistance
        public float finalResistanceMax = 10f;   // Maximum final resistance

        public HediffCompProperties_SecondThoughts()
        {
            this.compClass = typeof(HediffComp_SecondThoughts);
        }
    }

    public class HediffComp_SecondThoughts : HediffComp
    {
        public HediffCompProperties_SecondThoughts Props => (HediffCompProperties_SecondThoughts)this.props;

        public override void CompPostPostAdd(DamageInfo? dinfo)
        {
            base.CompPostPostAdd(dinfo);

            Pawn pawn = this.Pawn;
            if (pawn == null || pawn.guest == null) return;

            // Check if the pawn is a prisoner or slave
            if (pawn.guest.IsPrisoner || pawn.guest.IsSlave)
            {

                // Remove prisoner/slave status
                if (Props.removePrisonerStatus)
                {
                    pawn.guest.SetGuestStatus(null);
                }

                // Override "unwavering" mechanic by force-setting recruitability
                if (!pawn.guest.Recruitable)
                {
                    pawn.guest.Recruitable = true;
                }

                // Ensure they remain a prisoner at the end
                pawn.guest.SetGuestStatus(Faction.OfPlayer, GuestStatus.Prisoner);

                // Choose a random final resistance value within the range
                float finalResistance = Rand.Range(Props.finalResistanceMin, Props.finalResistanceMax);
                
                // INSTANT ZERO: Set both resistance and will to 0 when wavering loyalty triggers
                // This happens BEFORE setting the final resistance, so we zero them first
                pawn.guest.resistance = 0;
                
                // Reset will to 0 for slavery mechanics (if Ideology is active)
                if (ModsConfig.IdeologyActive)
                {
                    pawn.guest.will = 0;
                }
                
                // Then set the final resistance (which is 0-0 for sensory collapser, so stays 0)
                pawn.guest.resistance = finalResistance;

            }
        }
    }
}