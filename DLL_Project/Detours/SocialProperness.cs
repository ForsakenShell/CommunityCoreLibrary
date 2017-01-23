﻿using System;

using RimWorld;
using Verse;

namespace CommunityCoreLibrary.Detour
{

    internal static class _SocialProperness
    {

        // Fixes social properness for prison cells by checking that the thing/interaction
        // cells location is a prison cell instead of the same room
        // TODO: Check if it should return true if it's for a prisoner and the prisoner can't path
        [DetourMember( typeof( SocialProperness ) )]
        internal static bool                _IsSociallyProper( this Thing t, Pawn p, bool forPrisoner, bool animalsCare = false )
        {
            if(
                ( !animalsCare )&&
                ( !p.RaceProps.Humanlike )||
                ( !t.def.socialPropernessMatters )
            )
            {
                return true;
            }
            var thingPos = t.def.hasInteractionCell ? t.InteractionCell : t.Position;
            return( forPrisoner == thingPos.IsInPrisonCell( t.Map ) );
        }

    }

}
