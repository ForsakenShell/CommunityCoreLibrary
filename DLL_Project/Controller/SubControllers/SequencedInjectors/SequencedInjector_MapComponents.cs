﻿using System;
using System.Collections.Generic;
using System.Linq;

using Verse;

namespace CommunityCoreLibrary
{
    
    public class SequencedInjector_MapComponents : SequencedInjector
    {

        public override string              Name => "Map Components";

        public override bool                IsValid()
        {
            return true;
        }

        public override bool                Inject( InjectionSequence sequence, InjectionTiming timing )
        {
            if(
                ( sequence != InjectionSequence.GameLoad )||
                ( timing != InjectionTiming.MapComponents )||
                ( Find.Maps == null )||
                ( Find.Maps.Any( map => map.components == null ) ) // TODO: not sure if this should be "any" or "all"
            )
            {   // No error but only do it in the correct sequence and timing
                return true;
            }
            var existingComponents = ( from map in Find.Maps
                                       from component in map.components
                                       select component ).ToList();
            var injected = true;
            foreach( var mod in Controller.Data.Mods )
            {
                foreach( var assembly in mod.assemblies.loadedAssemblies )
                {
                    var mapComponentsForAssembly = assembly.GetTypes().Where( type => type.IsSubclassOf( typeof( MapComponent ) ) ).ToList();
                    if( !mapComponentsForAssembly.NullOrEmpty() )
                    {
                        foreach( var mapComponent in mapComponentsForAssembly )
                        {
                            if( !existingComponents.Any( c => c.GetType() == mapComponent ) )
                            {
                                var componentObject = (MapComponent) Activator.CreateInstance( mapComponent );
                                if( componentObject == null )
                                {
                                    CCL_Log.Trace(
                                        Verbosity.Injections,
                                        string.Format( "Unable to create instance of '{0}'", mapComponent.FullName ),
                                        Name
                                    );
                                    injected = false;
                                }
                                else
                                {
                                    CCL_Log.Trace(
                                        Verbosity.Injections,
                                        string.Format( "'{0}' injected into save game", mapComponent.FullName ),
                                        Name
                                    );
                                    existingComponents.Add( componentObject );
                                }
                            }
                        }
                    }
                }
            }
            return injected;
        }

    }

}
