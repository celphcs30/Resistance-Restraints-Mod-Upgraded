# Prisoner Restraints Upgraded

A fork of [Prisoner Restraints](https://steamcommunity.com/sharedfiles/filedetails/?id=3434403034) by SilkCircuit, updated for RimWorld 1.6.

## About

Unbreakable prisoners? Unwaveringly loyal captives? Not anymore. Prisoner Restraints introduces new methods to break down resistance and a progression system that can make unwaveringly loyal prisoners recruitable.

## Features

* **Restraining table** - Immobilizes prisoners and slowly reduces resistance. Hunger and sleep needs drop to negligible levels, ensuring minimal upkeep.
* **Humiliation cage** - Strips dignity, privacy, and apparel. Reduces prisoner resistance at a moderate rate. Effective, though prolonged use risks mental collapse.
* **Sensory collapser** - High-intensity deprivation chamber that bombards subjects with disorienting stimuli. Triggers early stages of Stockholm syndrome and only affects unwaveringly loyal prisoners.
* **Chemfuel bath** - Expedited resistance reduction through chemical burns. Requires frequent refueling to maintain effectiveness.

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

Prisoners can be manually released from any restraint device at any time using the "Release prisoner" button on the device's gizmo panel. When clicked, a warden will be assigned to remove the restraints and free the prisoner. The release process:

- Prevents the immobility hediff from being reapplied during release
- Forces the pawn to get up from the bed once restraints are removed
- Works on all restraint device types (Restraining table, Humiliation cage, Sensory collapser, Chemfuel bath)
- Provides audio feedback when buttons are clicked

## Changes from Original

* Updated for RimWorld 1.6 only (removed support for RimWorld 1.5)
* Added Vanilla Nutrient Paste Expanded support (nutrient paste dripper facility)
* Added Vanilla Chemfuel Expanded support (chemfuel pipe network integration)
* Implemented proper release system with interrupt handling to prevent hediff reapplication
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

