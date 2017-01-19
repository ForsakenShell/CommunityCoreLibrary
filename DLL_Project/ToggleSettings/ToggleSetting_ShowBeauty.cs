using System;

using RimWorld;
using Verse;

namespace CommunityCoreLibrary
{

    // TODO: replace this (probably ShowEnvironment)
    public class ToggleSetting_ShowBeauty : ToggleSetting
    {

        public override bool Value
        {
            get
            {
                return Find.PlaySettings.showEnvironment;
            }
            set
            {
                Find.PlaySettings.showEnvironment = value;
            }
        }

    }

}
