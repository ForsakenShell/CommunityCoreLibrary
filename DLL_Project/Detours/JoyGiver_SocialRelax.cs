using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;

namespace CommunityCoreLibrary.Detour
{

    internal static class _JoyGiver_SocialRelax
    {

        #region Detoured Methods

        internal static Job _TryGiveJobInt( this JoyGiver_SocialRelax obj, Pawn pawn, Predicate<CompGatherSpot> gatherSpotValidator )
        {
            var lister = pawn.Map.gatherSpotLister;

            if (lister.activeSpots.NullOrEmpty() )
            {
                return (Job)null;
            }

            var workingSpots = JoyGiver_SocialRelax_Extensions.WorkingSpots();
            var NumRadiusCells = JoyGiver_SocialRelax_Extensions.NumRadiusCells();
            var RadialPatternMiddleOutward = JoyGiver_SocialRelax_Extensions.RadialPatternMiddleOutward();

            workingSpots.Clear();
            for( int index = 0; index < lister.activeSpots.Count; ++index )
            {
                workingSpots.Add(lister.activeSpots[ index ] );
            }

            CompGatherSpot compGatherSpot;
            while( GenCollection.TryRandomElement<CompGatherSpot>( workingSpots, out compGatherSpot ) )
            {
                workingSpots.Remove( compGatherSpot );
                if(
                    ( !compGatherSpot.parent.IsForbidden( pawn ) )&&
                    ( pawn.CanReach(
                        compGatherSpot.parent,
                        PathEndMode.Touch,
                        Danger.None,
                        false ) )&&
                    ( compGatherSpot.parent.IsSociallyProper( pawn ) )&&
                    (
                        ( gatherSpotValidator == null )||
                        ( gatherSpotValidator( compGatherSpot ) )
                    )
                )
                {
                    var job = (Job)null;
                    if( compGatherSpot.parent.def.surfaceType == SurfaceType.Eat )
                    {
                        for( int index = 0; index < 30; ++index )
                        {
                            Building sittableThing = compGatherSpot.parent.RandomAdjacentCellCardinal().GetEdifice( compGatherSpot.parent.Map );
                            if(
                                ( sittableThing != null )&&
                                ( sittableThing.def.building.isSittable )&&
                                ( pawn.CanReserve( sittableThing, 1 ) )
                            )
                            {
                                job = new Job( JobDefOf.SocialRelax, compGatherSpot.parent, sittableThing );
                            }
                        }
                    }
                    else
                    {
                        for( int index = 0; index < RadialPatternMiddleOutward.Count; ++index )
                        {
                            Building sittableThing = ( compGatherSpot.parent.Position + RadialPatternMiddleOutward[ index ] )
                                .GetEdifice( compGatherSpot.parent.Map );
                            if(
                                ( sittableThing != null )&&
                                ( sittableThing.def.building.isSittable )&&
                                (
                                    ( pawn.CanReserve(sittableThing, 1 ) )&&
                                    ( !sittableThing.IsForbidden( pawn ) )&&
                                    ( GenSight.LineOfSight(
                                        compGatherSpot.parent.Position,
                                        sittableThing.Position,
                                        sittableThing.Map,
                                        true ) )
                                )
                            )
                            {
                                job = new Job( JobDefOf.SocialRelax, compGatherSpot.parent, sittableThing );
                                break;
                            }
                        }
                        if( job == null )
                        {
                            for( int index = 0; index < 30; ++index )
                            {
                                IntVec3 occupySpot = compGatherSpot.parent.Position + GenRadial.RadialPattern[ Rand.Range( 1, NumRadiusCells ) ];
                                if(
                                    ( pawn.CanReserveAndReach(
                                        occupySpot,
                                        PathEndMode.OnCell,
                                        Danger.None,
                                        1 ) )&&
                                    ( occupySpot.GetEdifice( compGatherSpot.parent.Map ) == null )&&
                                    ( GenSight.LineOfSight(
                                        compGatherSpot.parent.Position,
                                        occupySpot,
                                        compGatherSpot.parent.Map,
                                        true ) )
                                )
                                {
                                    job = new Job( JobDefOf.SocialRelax, compGatherSpot.parent, occupySpot );
                                }
                            }
                        }
                    }
                    if( job == null )
                    {
                        return (Job)null;
                    }
                    if(
                        ( pawn.health.capacities.CapableOf( PawnCapacityDefOf.Manipulation ) )&&
                        (
                            ( pawn.story == null )||
                            ( pawn.story.traits.DegreeOfTrait( TraitDefOf.DrugDesire ) >= 0 )
                        )
                    )
                    {
                        List<Thing> list = pawn.Map.listerThings.AllThings.Where( t => (
                            ( t.def.IsAlcohol() )||
                            ( t is Building_AutomatedFactory )
                        ) ).ToList();
                        if( list.Count > 0 )
                        {
                            Thing thing = GenClosest.ClosestThing_Global_Reachable(
                                compGatherSpot.parent.Position,
                                pawn.Map,
                                list,
                                PathEndMode.OnCell,
                                TraverseParms.For(
                                    pawn,
                                    pawn.NormalMaxDanger() ),
                                40f,
                                ( t ) =>
                            {
                                if( t.IsForbidden( pawn ) )
                                {
                                    return false;
                                }

                                if( t is Building_AutomatedFactory )
                                {
                                    var FS = t as Building_AutomatedFactory;
                                    if(
                                        ( !FS.InteractionCell.Standable( t.Map ) )||
                                        ( !FS.CompPowerTrader.PowerOn )||
                                        ( FS.BestProduct( FoodSynthesis.IsAlcohol, FoodSynthesis.SortAlcohol ) == null )
                                    )
                                    {
                                        return false;
                                    }
                                }
                                return pawn.CanReserve( t, 1 );
                            } );
                            if( thing != null )
                            {
                                job.targetC = thing;
                                job.count = Mathf.Min( thing.stackCount, thing.def.ingestible.maxNumToIngestAtOnce );
                            }
                        }
                    }
                    return job;
                }
            }
            return (Job)null;
        }

        #endregion

    }

}
