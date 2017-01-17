using System;
using System.Collections.Generic;
using System.Linq;

using RimWorld;
using Verse;

namespace CommunityCoreLibrary
{

    public class Alert_PasteDispenserNeedsHopper : RimWorld.Alert_PasteDispenserNeedsHopper
    {

        public override AlertReport GetReport()
        {
            var dispensers = from map in Find.Maps
                             from building in map.listerBuildings.allBuildingsColonist
                             where building is Building_NutrientPasteDispenser
                             select building;

            foreach (var dispenser in dispensers)
            {
                var CompHopperUser = dispenser.TryGetComp<CompHopperUser>();

                if (CompHopperUser == null || CompHopperUser.FindHoppers().NullOrEmpty())
                {
                    continue;
                }

                var edifices = from cell in GenAdj.CellsAdjacentCardinal(dispenser)
                               from map in Find.Maps
                               select GridsUtility.GetEdifice(cell, map);

                if (edifices.Any(edifice => edifice != null && edifice.def == ThingDefOf.Hopper))
                {
                    return AlertReport.CulpritIs(dispenser);
                }
            }

            return AlertReport.Inactive;
        }
    }
}
