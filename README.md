# Prisoner Restraints Upgraded

A fork of [Prisoner Restraints](https://steamcommunity.com/sharedfiles/filedetails/?id=3434403034) by SilkCircuit, updated for RimWorld 1.6.

## About

Unbreakable prisoners? Unwaveringly loyal captives? Not anymore. Prisoner Restraints introduces new methods to break down resistance and a progression system that can make unwaveringly loyal prisoners recruitable.

## Features

* **Restraining table** - Immobilizes prisoners and slowly reduces resistance and will. Hunger and sleep needs drop to negligible levels, ensuring minimal upkeep. Can be placed in individual prison cells or barracks.
* **Humiliation cage** - Strips dignity, privacy, and apparel. Reduces prisoner resistance and will at a moderate rate. Effective, though prolonged use risks mental collapse. Can be placed in individual prison cells or barracks.
* **Sensory collapser** - High-intensity deprivation chamber that bombards subjects with disorienting stimuli. Triggers early stages of Stockholm syndrome and only affects unwaveringly loyal prisoners. When unwaveringly loyal status is removed, instantly zeros both resistance and will. Can be placed in individual prison cells or barracks.
* **Chemfuel bath** - Expedited resistance and will reduction through chemical burns. Requires frequent refueling to maintain effectiveness. Can be placed in individual prison cells or barracks.

## Resistance & Will Reduction

All restraint devices reduce both **resistance** (for recruitment) and **will** (for slavery) proportionally, ensuring both stats reach zero at the same time. This makes prisoners ready for either recruitment or enslavement.

**How it works:**
- The reduction rate is calculated proportionally based on current resistance and will values
- If a prisoner has 85 will and 14.1 resistance, both will be reduced at proportional rates so they reach zero simultaneously
- The immobile multiplier applies to the total reduction, then split proportionally between both stats
- **Will reduction requires the Ideology DLC** - if Ideology is not active, only resistance is reduced

**Sensory Collapser special behavior:**
- When the unwaveringly loyal status is removed (Stockholm syndrome triggers), both resistance and will are instantly set to zero
- The prisoner is automatically released at this moment (no manual release needed)

## Requirements

* RimWorld 1.6
* [Harmony](https://github.com/pardeike/HarmonyRimWorld) (mod dependency)

## Vanilla Nutrient Paste Expanded Support

All restraint beds can now accept the nutrient paste dripper facility. Place a dripper adjacent to the bed head to automatically feed prisoners via the paste network. This integration is added automatically when [Vanilla Nutrient Paste Expanded](https://steamcommunity.com/sharedfiles/filedetails/?id=2920385763) is installed.

Supported beds:
* Restraining table
* Humiliation cage
* Sensory collapser
* Chemfuel bath

## Vanilla Chemfuel Expanded Support

The chemfuel bath can now be connected to the chemfuel pipe network for automatic refueling. This integration is added automatically when [Vanilla Chemfuel Expanded](https://steamcommunity.com/sharedfiles/filedetails/?id=2792917473) is installed.

Supported beds:
* Chemfuel bath

## Release System

**Sensory Collapser (Automatic):**
- Automatically releases prisoners when unwaveringly loyal status is removed
- This occurs when the device successfully triggers Stockholm syndrome (wavering loyalty hediff)
- Both resistance and will are instantly set to zero at this moment
- No manual intervention required

**Other Devices (Manual):**
- Restraining table, Humiliation cage, and Chemfuel bath require manual release
- Use the "Release prisoner" button on the device's gizmo panel
- A warden will be assigned to remove restraints and free the prisoner
- The release process prevents the immobility hediff from being reapplied during release
- Forces the pawn to get up from the bed once restraints are removed
- Provides audio feedback when buttons are clicked

**Re-trapping Prevention:**
- Once a prisoner is released from any restraint device, they are automatically unassigned from that bed
- Released prisoners are protected from being re-trapped in any restraint device (all types: Restraining table, Humiliation cage, Sensory collapser, Chemfuel bath)
- This prevents the edge case where released prisoners would return to their assigned bed and get trapped again
- Works correctly in both individual prison cells and barracks (where multiple beds may be present)
- Protection is cleared when the prisoner leaves all restraint beds, allowing reuse if needed

## Mod Settings

Adjustable reduction speed multipliers for each device type (except Sensory Collapser, which uses instant zero):

- **Restraining Table Multiplier**: 0.1x to 5.0x (default: 1.0x)
- **Humiliation Cage Multiplier**: 0.1x to 5.0x (default: 1.0x)
- **Chemfuel Bath Multiplier**: 0.1x to 5.0x (default: 1.0x)

Changes take effect immediately without reloading. Access via Options → Mod Settings → Prisoner Restraints.

## Changes from Original

* Updated for RimWorld 1.6 only (removed support for RimWorld 1.5)
* **Added will reduction for slavery mechanics** - all devices now reduce both resistance and will proportionally
* **Sensory collapser instant zero and auto-release** - when unwaveringly loyal is removed, instantly zeros both resistance and will, then automatically releases the prisoner
* **Mod Settings** - configurable reduction speed multipliers for Restraining Table, Humiliation Cage, and Chemfuel Bath
* Added Vanilla Nutrient Paste Expanded support (nutrient paste dripper facility)
* Added Vanilla Chemfuel Expanded support (chemfuel pipe network integration)
* Implemented proper release system with interrupt handling to prevent hediff reapplication
* **Re-trapping prevention** - Released prisoners are automatically unassigned and protected from being re-trapped in any restraint device (works in both cells and barracks)
* Added audio feedback to gizmo buttons
* Fixed race condition where immobility hediff would be instantly reapplied after removal
* Updated Harmony dependency configuration
* Cleaned up mod metadata and naming

## Installation

1. Download or clone this repository
2. Place the `ResistanceRestraintsMod` folder in your RimWorld `Mods` directory:
   - Steam: `C:\Program Files (x86)\Steam\steamapps\common\RimWorld\Mods\`
   - Or your RimWorld installation's `Mods` folder
3. Enable the mod in RimWorld's mod menu

## Credits

Original mod by [SilkCircuit](https://steamcommunity.com/id/SilkCircuit/)  
Forked and updated for RimWorld 1.6 by celphcs30

## License

MIT License - See LICENSE file for details.

