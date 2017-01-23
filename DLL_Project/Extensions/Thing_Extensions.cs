﻿using System;
using System.Collections.Generic;
using System.Linq;

using Verse;

namespace CommunityCoreLibrary
{

    public static class Thing_Extensions
    {

        #region Cell Scanners

        // Returns true if there is another thing in the cell with the same def
        public static bool                  IsSameThingDefInCell( this Thing thing, IntVec3 cell )
        {
            var thingsInCell = cell.GetThingList( thing.Map );
            if( thingsInCell.NullOrEmpty() )
            {
                return false;
            }
            return thingsInCell
                .Any( t =>
                    ( t != thing )&&
                    ( t.def == thing.def )
                );
        }

        // Returns true if there is another thing in the cell with the same graphic linker
        public static bool                  IsSameGraphicLinkerInCell( this Thing thing, IntVec3 cell )
        {
            var thingsInCell = cell.GetThingList( thing.Map );
            if( thingsInCell.NullOrEmpty() )
            {
                return false;
            }
            return thingsInCell
                .Any( t =>
                    ( t != thing )&&
                    ( t.def.graphicData != null )&&
                    ( t.def.graphicData.linkFlags == thing.def.graphicData.linkFlags )
                );
        }

        // Returns true if there is another thing in the cell with the same thing comp
        public static bool                  IsSameThingCompInCell( this Thing thing, IntVec3 cell, Type MatchingComp )
        {
            var thingsInCell = cell.GetThingList( thing.Map );
            if( thingsInCell.NullOrEmpty() )
            {
                return false;
            }
            return thingsInCell
                .Any( t =>
                    ( t != thing )&&
                    ( ( t as ThingWithComps ) != null )&&
                    ( ( (ThingWithComps)t ).AllComps
                        .Any( tc =>
                            ( tc.GetType() == MatchingComp )
                        ) )
                );
        }

        #endregion

        #region Cell Listers

        // Returns a list of things in the cell with the same def
        public static List< Thing >         ListSameThingDefInCell( this Thing thing, IntVec3 cell )
        {
            var thingsInCell = cell.GetThingList( thing.Map );
            if( thingsInCell.NullOrEmpty() )
            {
                return null;
            }
            return thingsInCell
                .Where( t =>
                    ( t != thing )&&
                    ( t.def == thing.def )
                ).ToList();
        }

        // Returns a list of things in the cell with the same graphic linker
        public static List< Thing >         ListSameGraphicLinkerInCell( this Thing thing, IntVec3 cell )
        {
            var thingsInCell = cell.GetThingList( thing.Map );
            if( thingsInCell.NullOrEmpty() )
            {
                return null;
            }
            return thingsInCell
                .Where( t =>
                    ( t != thing )&&
                    ( t.def.graphicData != null )&&
                    ( t.def.graphicData.linkFlags == thing.def.graphicData.linkFlags )
                ).ToList();
        }

        // Returns a list of things in the cell with the same thing comp
        public static List< Thing >         ListSameThingCompInCell( this Thing thing, IntVec3 cell, Type MatchingComp )
        {
            var thingsInCell = cell.GetThingList( thing.Map );
            if( thingsInCell.NullOrEmpty() )
            {
                return null;
            }
            return thingsInCell
                .Where( t =>
                    ( t != thing )&&
                    ( ( t as ThingWithComps ) != null )&&
                    ( ( (ThingWithComps)t ).AllComps
                        .Any( tc =>
                            ( tc.GetType() == MatchingComp )
                        ) )
                ).ToList();
        }

        #endregion

        #region Room Scanners

        // Returns true if there is another thing in the room with the same def
        public static bool                  IsSameThingDefInRoom( this Thing thing )
        {
            // Get room cells
            var cells = thing.GetRoom().Cells.ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Things in cell with matching def
                if( thing.IsSameThingDefInCell( cell ) )
                {
                    return true;
                }
            }

            // Nothing found
            return false;
        }

        // Returns true if there is another thing in the room with the same graphic linker
        public static bool                  IsSameGraphicLinkerInRoom( this Thing thing )
        {
            // Get adjacent cells
            var cells = thing.GetRoom().Cells.ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Things in cell with matching graphic linker
                if( thing.IsSameGraphicLinkerInCell( cell ) )
                {
                    return true;
                }
            }

            // Nothing found
            return false;
        }

        // Returns true if there is another thing in the room with the same thing comp
        public static bool                  IsSameThingCompInRoom( this Thing thing, Type MatchingComp )
        {
            // Get adjacent cells
            var cells = thing.GetRoom().Cells.ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Things in cell with matching thing comp
                if( thing.IsSameThingCompInCell( cell, MatchingComp ) )
                {
                    return true;
                }
            }

            // Nothing found
            return false;
        }

        #endregion

        #region Room Listers

        // Get's a group of things in the room with the same def
        public static List< Thing >         ListSameThingDefInRoom( this Thing thing )
        {
            // List for things
            var list = new List< Thing >();

            // Get room cells
            var cells = thing.GetRoom().Cells.ToList();

            // Scan cells
            foreach( IntVec3 cell in cells )
            {
                // Add things in cell with matching def
                var things = thing.ListSameThingDefInCell( cell );
                if( !things.NullOrEmpty() )
                {
                    list.AddRangeUnique( things );
                }
            }

            // Return list or null if empty
            return list.Count == 0 ? null : list;
        }

        // Get's a group of things in the room with the same graphic linker
        public static List< Thing >         ListSameGraphicLinkerInRoom( this Thing thing )
        {
            // List for things
            var list = new List< Thing >();

            // Get room cells
            var cells = thing.GetRoom().Cells.ToList();

            // Scan cells
            foreach( IntVec3 cell in cells )
            {
                // Add things in cell with matching graphic linker
                var things = thing.ListSameGraphicLinkerInCell( cell );
                if( !things.NullOrEmpty() )
                {
                    list.AddRangeUnique( things );
                }
            }

            // Return list or null if empty
            return list.Count == 0 ? null : list;
        }

        // Get's a group of things in room with the same thing comp
        public static List< Thing >         ListSameThingCompInRoom( this Thing thing, Type MatchingComp )
        {
            // List for things
            var list = new List< Thing >();

            // Get room cells
            var cells = thing.GetRoom().Cells.ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Add things in cell with matching thing comp
                var things = thing.ListSameThingCompInCell( cell, MatchingComp );
                if( ( things != null )&&
                    ( things.Count > 0 ) )
                {
                    list.AddRangeUnique( things );
                }
            }

            // Return list or null if empty
            return list.Count == 0 ? null : list;
        }

        #endregion

        #region Touching Scanners

        // Returns true if there is another thing touching it with the same def
        public static bool                  IsSameThingDefTouching( this Thing thing )
        {
            // Get adjacent cells
            var cells = GenAdj.CellsAdjacentCardinal( thing ).ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Things in cell with matching def
                if( thing.IsSameThingDefInCell( cell ) )
                {
                    return true;
                }
            }

            // Nothing found
            return false;
        }

        // Returns true if there is another thing touching it with the same linker flag
        public static bool                  IsSameGraphicLinkerTouching( this Thing thing )
        {
            // Get adjacent cells
            var cells = GenAdj.CellsAdjacentCardinal( thing ).ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Things in cell with matching graphic linker
                if( thing.IsSameGraphicLinkerInCell( cell ) )
                {
                    return true;
                }
            }

            // Nothing found
            return false;
        }

        // Returns if this thing has another thing touching it with the same thing comp
        public static bool                  IsSameThingCompTouching( this Thing thing, Type MatchingComp )
        {
            // Get adjacent cells
            var cells = GenAdj.CellsAdjacentCardinal( thing ).ToList();

            // Scan cells
            foreach( var cell in cells )
            {
                // Things in cell with matching thing comp
                if( thing.IsSameThingCompInCell( cell, MatchingComp ) )
                {
                    return true;
                }
            }

            // Nothing found
            return false;
        }

        #endregion

        #region Touching Listers

        // Returns a list of touching things with the same def
        // optionally room bounds
        public static List< Thing >         ListSameThingDefTouching( this Thing thing, bool RoomBound = false, List< Thing > cache = null )
        {
            if( cache == null )
            {
                // Create cache on first call
                cache = new List< Thing >();
            }

            if( cache.Contains( thing ) )
            {
                // Already in list
                return cache;
            }

            // Add it to the list
            cache.Add( thing );

            List< IntVec3 > cells;
            if( RoomBound )
            {
                // Get adjacent cells bound by room
                cells = GenAdj.CellsAdjacentCardinal( thing )
                    .Where( c =>
                        ( GridsUtility.GetRoom( thing ) == GridsUtility.GetRoom( c, thing.Map ) )
                    )
                    .ToList();
            }
            else
            {
                // Get all adjacent cells
                cells = GenAdj.CellsAdjacentCardinal( thing )
                    .ToList();
            }

            // Scan cells
            foreach( IntVec3 cell in cells )
            {
                // Add things in cell with matching def
                var things = thing.ListSameThingDefInCell( cell );
                if( ( things != null )&&
                    ( things.Count > 0 ) )
                {
                    // Scan things
                    foreach( Thing match in things )
                    {
                        cache = match.ListSameThingDefTouching( RoomBound, cache );
                    }
                }
            }
            return cache;
        }

        // Returns a list of touching things with the same graphic linker
        // optionally room bounds
        public static List< Thing >         ListSameGraphicLinkerTouching( this Thing thing, bool RoomBound = false, List< Thing > cache = null )
        {
            if( cache == null )
            {
                // Create cache on first call
                cache = new List< Thing >();
            }

            if( cache.Contains( thing ) )
            {
                // Already in list
                return cache;
            }

            // Add it to the list
            cache.Add( thing );

            List< IntVec3 > cells;
            if( RoomBound )
            {
                // Get adjacent cells bound by room
                cells = GenAdj.CellsAdjacentCardinal( thing )
                    .Where( c =>
                        ( c.GetRoom( thing.Map ) == thing.GetRoom() )
                    )
                    .ToList();
            }
            else
            {
                // Get all adjacent cells
                cells = GenAdj.CellsAdjacentCardinal( thing )
                    .ToList();
            }

            // Scan cells
            foreach( IntVec3 cell in cells )
            {
                // Add things in cell with matching graphic linker
                var things = thing.ListSameGraphicLinkerInCell( cell );
                if( ( things != null )&&
                    ( things.Count > 0 ) )
                {
                    // Scan things
                    foreach( Thing match in things )
                    {
                        cache = match.ListSameGraphicLinkerTouching( RoomBound, cache );
                    }
                }
            }
            return cache;
        }

        // Returns a list of touching things by thing comp
        // optionally room bounds
        public static List< Thing >         ListSameThingCompTouching( this Thing thing, Type MatchingComp, bool RoomBound = false, List< Thing > cache = null )
        {
            if( cache == null )
            {
                // Create cache on first call
                cache = new List< Thing >();
            }

            if( cache.Contains( thing ) )
            {
                // Already in list
                return cache;
            }

            // Add it to the list
            cache.Add( thing );

            List< IntVec3 > cells;
            if( RoomBound )
            {
                // Get adjacent cells bound by room
                cells = GenAdj.CellsAdjacentCardinal( thing )
                    .Where( c =>
                        ( c.GetRoom( thing.Map ) == thing.GetRoom() )
                    )
                    .ToList();
            }
            else
            {
                // Get all adjacent cells
                cells = GenAdj.CellsAdjacentCardinal( thing )
                    .ToList();
            }

            // Scan cells
            foreach( IntVec3 cell in cells )
            {
                // Add things in cell with matching thing comp
                var things = thing.ListSameThingCompInCell( cell, MatchingComp );
                if( ( things != null )&&
                    ( things.Count > 0 ) )
                {
                    // Scan things
                    foreach( Thing match in things )
                    {
                        cache = match.ListSameThingCompTouching( MatchingComp, RoomBound, cache );
                    }
                }
            }
            return cache;
        }

        #endregion

        public static bool PawnHasJobUsing( this Thing thing, Pawn pawn )
        {
            if(
                ( pawn == null ) ||
                ( pawn.CurJob == null )
            )
            {
                return false;
            }
            if(
                ( pawn.CurJob.targetA != null ) &&
                ( pawn.CurJob.targetA.Thing == thing )
            )
            {
                return true;
            }
            if(
                ( pawn.CurJob.targetB != null ) &&
                ( pawn.CurJob.targetB.Thing == thing )
            )
            {
                return true;
            }
            if(
                ( pawn.CurJob.targetC != null ) &&
                ( pawn.CurJob.targetC.Thing == thing )
            )
            {
                return true;
            }
            return false;
        }

        public static Pawn GetPawnUsing( this Thing thing )
        {
            if( !thing.def.hasInteractionCell )
            {
                return null;
            }
            var pawnThings = thing.InteractionCell.GetThingList( thing.Map ).Where( t => t is Pawn ).ToList();
            if( pawnThings.NullOrEmpty() )
            {
                return null;
            }
            foreach( var pawnThing in pawnThings )
            {
                if( thing.PawnHasJobUsing( (Pawn)pawnThing ) )
                {
                    return (Pawn)pawnThing;
                }
            }
            return null;
        }

        public static List<Pawn> GetOccupyingPawnsUsing( this Thing thing )
        {
            var pawnsUsing = new List<Pawn>();
            var occupiedRect = thing.OccupiedRect();
            foreach( var cell in occupiedRect )
            {
                var pawnThings = cell.GetThingList( thing.Map ).Where( t => t is Pawn ).ToList();
                if( !pawnThings.NullOrEmpty() )
                {
                    foreach( var pawnThing in pawnThings )
                    {
                        if( thing.PawnHasJobUsing( (Pawn)pawnThing ) )
                        {
                            pawnsUsing.AddUnique( (Pawn)pawnThing );
                        }
                    }
                }
            }
            return pawnsUsing;
        }

    }

}
