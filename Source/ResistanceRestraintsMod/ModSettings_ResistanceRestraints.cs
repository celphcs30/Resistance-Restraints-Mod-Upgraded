using Verse;

namespace ResistanceRestraintsMod
{
    public class ModSettings_ResistanceRestraints : ModSettings
    {
        // Reduction speed multipliers for each device type
        // 1.0 = default speed, 2.0 = twice as fast, 0.5 = half speed
        public float restrainingTableMultiplier = 1.0f;
        public float humiliationCageMultiplier = 1.0f;
        public float chemfuelBathMultiplier = 1.0f;

        public static ModSettings_ResistanceRestraints Settings;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref restrainingTableMultiplier, "restrainingTableMultiplier", 1.0f);
            Scribe_Values.Look(ref humiliationCageMultiplier, "humiliationCageMultiplier", 1.0f);
            Scribe_Values.Look(ref chemfuelBathMultiplier, "chemfuelBathMultiplier", 1.0f);
            base.ExposeData();
        }
    }
}

