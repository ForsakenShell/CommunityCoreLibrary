using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;   // Always needed
using RimWorld;      // RimWorld specific functions are found here
using Verse;         // RimWorld universal objects are here
//using Verse.AI;    // Needed when you do something with the AI
//using Verse.Sound; // Needed when you do something with the Sound

namespace CommunityCoreLibrary
{
    public class Alert_HopperNeedsBuilding : Alert
    {
        public override AlertPriority Priority
        {
            get
            {
                return AlertPriority.High;
            }
        }

        public override AlertReport GetReport()
        {
            var buildings = from map in Find.Maps
                            from building in map.listerBuildings.allBuildingsColonist
                            where building.def.IsHopper()
                            select building;

            foreach (var building in buildings)
            {
                var hopperComp = building.GetComp<CompHopper>();
                if (hopperComp.FindHopperUser() == null)
                {
                    this.defaultExplanation = "Alert_HopperNeedsBuilding_Description".Translate(building.def.label);
                    return AlertReport.CulpritIs(building);
                }
            }

            return AlertReport.Inactive;
        }

        public Alert_HopperNeedsBuilding()
        {
            this.defaultLabel = "Alert_HopperNeedsBuilding_Label".Translate();
        }
    }
}
