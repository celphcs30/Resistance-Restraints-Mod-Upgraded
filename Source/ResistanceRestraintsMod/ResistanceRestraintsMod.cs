using RimWorld;
using UnityEngine;
using Verse;

namespace ResistanceRestraintsMod
{
    public class ResistanceRestraintsMod : Mod
    {
        public ResistanceRestraintsMod(ModContentPack content) : base(content)
        {
            ModSettings_ResistanceRestraints.Settings = GetSettings<ModSettings_ResistanceRestraints>();
        }

        public override string SettingsCategory()
        {
            return "Prisoner Restraints";
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            if (ModSettings_ResistanceRestraints.Settings == null)
            {
                ModSettings_ResistanceRestraints.Settings = GetSettings<ModSettings_ResistanceRestraints>();
            }

            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Reduction Speed Multipliers");
            listing.GapLine();

            // Restraining Table
            listing.Label($"Restraining Table: {ModSettings_ResistanceRestraints.Settings.restrainingTableMultiplier.ToString("F2")}x speed");
            ModSettings_ResistanceRestraints.Settings.restrainingTableMultiplier = listing.Slider(ModSettings_ResistanceRestraints.Settings.restrainingTableMultiplier, 0.1f, 5.0f);
            listing.Gap(12f);

            // Humiliation Cage
            listing.Label($"Humiliation Cage: {ModSettings_ResistanceRestraints.Settings.humiliationCageMultiplier.ToString("F2")}x speed");
            ModSettings_ResistanceRestraints.Settings.humiliationCageMultiplier = listing.Slider(ModSettings_ResistanceRestraints.Settings.humiliationCageMultiplier, 0.1f, 5.0f);
            listing.Gap(12f);

            // Chemfuel Bath
            listing.Label($"Chemfuel Bath: {ModSettings_ResistanceRestraints.Settings.chemfuelBathMultiplier.ToString("F2")}x speed");
            ModSettings_ResistanceRestraints.Settings.chemfuelBathMultiplier = listing.Slider(ModSettings_ResistanceRestraints.Settings.chemfuelBathMultiplier, 0.1f, 5.0f);
            listing.Gap(12f);

            listing.Label("Note: Sensory Collapser uses instant zero and is not affected by these settings.");
            listing.Gap(12f);

            if (listing.ButtonText("Reset to Default"))
            {
                ModSettings_ResistanceRestraints.Settings.restrainingTableMultiplier = 1.0f;
                ModSettings_ResistanceRestraints.Settings.humiliationCageMultiplier = 1.0f;
                ModSettings_ResistanceRestraints.Settings.chemfuelBathMultiplier = 1.0f;
            }

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }
    }
}

