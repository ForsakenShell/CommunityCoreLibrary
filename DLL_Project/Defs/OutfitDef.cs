using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace CommunityCoreLibrary
{
    using CommunityCoreLibrary.Detour;

    public class OutfitDef : Def
    {
        /*
                        <li>
                            <label>Nudist</label>
                            <filter>
                                <disallowedSpecialFilters>
                                    <li>AllowCorpsesColonist</li>
                                    <li>AllowCorpsesStranger</li>
                                    <li>AllowRotten</li>
                                    <li>AllowNonSmeltableWeapons</li>
                                </disallowedSpecialFilters>
                                <allowedDefs>
                                    <li>Apparel_CowboyHat</li>
                                    <li>Apparel_Tuque</li>
                                    <li>Apparel_MilitaryHelmet</li>
                                </allowedDefs>
                                <allowedHitPointsPercents>0~1</allowedHitPointsPercents>
                                <allowedQualityLevels>Awful~Legendary</allowedQualityLevels>
                            </filter>
                        </li>
        */
        #region XML Data

        public string                               label               = "";
        public ThingFilter                          filter              = new ThingFilter();

        #endregion

        public static OutfitDef Named(string defName)
        {
            return DefDatabase<OutfitDef>.GetNamed(defName);
        }

        public override void ResolveReferences()
        {
            base.ResolveReferences();

            _OutfitDatabase.OutfitDefs.Add(this);
        }
    }
}
