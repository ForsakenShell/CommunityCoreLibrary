﻿using System;

using RimWorld;
using Verse;

namespace CommunityCoreLibrary.Detour
{

    internal static class _ThingSelectionUtility
    {
        
        internal static bool _SelectableNow( this Thing t )
        {
            if(
                ( !t.def.selectable )||
                ( !t.Spawned )||
                ( HideItemManager.PreventItemSelection( t ) )
            )
            {
                return false;
            }
            if(
                ( t.def.size.x == 1 )&&
                ( t.def.size.z == 1 )
            )
            {
                return !t.Position.Fogged( t.Map );
            }
            foreach( var cell in t.OccupiedRect() )
            {
                if( !cell.Fogged( t.Map ) )
                {
                    return true;
                }
            }
            return false;
        }

    }

}
