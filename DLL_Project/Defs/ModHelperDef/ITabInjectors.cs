using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Verse;

namespace CommunityCoreLibrary
{

    public class MHD_InspectTabBases : IInjector
    {

#if DEBUG
        public string                       InjectString
        {
            get
            {
                return "InspectTabBases injected";
            }
        }

        public bool                         IsValid( ModHelperDef def, ref string errors )
        {
            if( def.InspectTabBases.NullOrEmpty() )
            {
                return true;
            }

            bool isValid = true;

            for( var InspectTabBaseIndex = 0; InspectTabBaseIndex < def.InspectTabBases.Count; InspectTabBaseIndex++ )
            {
                var qualifierValid = true;
                var injectionSet = def.InspectTabBases[ InspectTabBaseIndex ];
                if(
                    ( !injectionSet.requiredMod.NullOrEmpty() )&&
                    ( Find_Extensions.ModByName( injectionSet.requiredMod ) == null )
                )
                {
                    continue;
                }
                var replaceTabIsValid = true;
                if(
                    ( injectionSet.newInspectTabBase == null )||
                    ( !injectionSet.newInspectTabBase.IsSubclassOf( typeof( InspectTabBase ) ) )
                )
                {
                    errors += string.Format("Unable to resolve InspectTabBase '{0}'", injectionSet.newInspectTabBase );
                    isValid = false;
                }
                if(
                    ( injectionSet.replaceInspectTabBase != null )&&
                    ( !injectionSet.replaceInspectTabBase.IsSubclassOf( typeof( InspectTabBase ) ) )
                )
                {
                    errors += string.Format("Unable to resolve InspectTabBase '{0}'", injectionSet.replaceInspectTabBase );
                    isValid = false;
                    replaceTabIsValid = false;
                }
                if(
                    ( injectionSet.targetDefs.NullOrEmpty() )&&
                    ( injectionSet.qualifier == null )
                )
                {
                    errors += "targetDefs and qualifier are both null, one or the other must be supplied";
                    isValid = false;
                    qualifierValid = false;
                }
                if(
                    ( !injectionSet.targetDefs.NullOrEmpty() )&&
                    ( injectionSet.qualifier != null )
                )
                {
                    errors += "targetDefs and qualifier are both supplied, only one or the other must be supplied";
                    isValid = false;
                    qualifierValid = false;
                }
                if( qualifierValid )
                {
                    if( !injectionSet.targetDefs.NullOrEmpty() )
                    {
                        for( var index = 0; index < injectionSet.targetDefs.Count; index++ )
                        {
                            if( injectionSet.targetDefs[ index ].NullOrEmpty() )
                            {
                                errors += string.Format( "targetDef in InspectTabBases is null or empty at index {0}", index.ToString() );
                                isValid = false;
                            }
                            else
                            {
                                var thingDef = DefDatabase<ThingDef>.GetNamed( injectionSet.targetDefs[ index ], false );
                                if( thingDef == null )
                                {
                                    errors += string.Format( "Unable to resolve targetDef '{0}'", injectionSet.targetDefs[ index ] );
                                    isValid = false;
                                }
                                else if(
                                    ( injectionSet.replaceInspectTabBase != null )&&
                                    ( replaceTabIsValid )
                                )
                                {
                                    if( !CanReplaceOn( thingDef, injectionSet.replaceInspectTabBase ) )
                                    {
                                        errors += string.Format("targetDef '{0}' does not contain InspectTabBase '{1}' to replace", injectionSet.targetDefs[ index ], injectionSet.replaceInspectTabBase );
                                        isValid = false;
                                    }
                                }
                            }
                        }
                    }
                    if( injectionSet.qualifier != null )
                    {
                        if( !injectionSet.qualifier.IsSubclassOf( typeof( DefInjectionQualifier ) ) )
                        {
                            errors += string.Format( "Unable to resolve qualifier '{0}'", injectionSet.qualifier );
                            isValid = false;
                        }
                        else if(
                                ( injectionSet.replaceInspectTabBase != null )&&
                                ( replaceTabIsValid )
                            )
                        {
                            var thingDefs = DefInjectionQualifier.FilteredThingDefs( injectionSet.qualifier, ref injectionSet.qualifierInt, null );
                            if( !thingDefs.NullOrEmpty() )
                            {
                                foreach( var thingDef in thingDefs )
                                {
                                    if( !CanReplaceOn( thingDef, injectionSet.replaceInspectTabBase ) )
                                    {
                                        errors += string.Format("qualified ThingDef '{0}' does not contain InspectTabBase '{1}' to replace", thingDef.defName, injectionSet.replaceInspectTabBase );
                                        isValid = false;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return isValid;
        }

        private bool                        CanReplaceOn( ThingDef thingDef, Type replaceInspectTabBase )
        {
            return(
                ( !thingDef.inspectorTabs.NullOrEmpty() )&&
                ( thingDef.inspectorTabs.Contains( replaceInspectTabBase ) )
            );
        }
#endif

        public bool                         Injected( ModHelperDef def )
        {
            if( def.InspectTabBases.NullOrEmpty() )
            {
                return true;
            }

            for( var index = 0; index < def.InspectTabBases.Count; index++ )
            {
                var injectionSet = def.InspectTabBases[ index ];
                if(
                    ( !injectionSet.requiredMod.NullOrEmpty() )&&
                    ( Find_Extensions.ModByName( injectionSet.requiredMod ) == null )
                )
                {
                    continue;
                }
                var thingDefs = DefInjectionQualifier.FilteredThingDefs( injectionSet.qualifier, ref injectionSet.qualifierInt, injectionSet.targetDefs );
                if( !thingDefs.NullOrEmpty() )
                {
                    foreach( var thingDef in thingDefs )
                    {
                        if(
                            ( thingDef.inspectorTabs.NullOrEmpty() )||
                            ( !thingDef.inspectorTabs.Contains( injectionSet.newInspectTabBase ) )
                        )
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public bool                         Inject( ModHelperDef def )
        {
            if( def.InspectTabBases.NullOrEmpty() )
            {
                return true;
            }

            for( var index = 0; index < def.InspectTabBases.Count; index ++ )
            {
                var injectionSet = def.InspectTabBases[ index ];
                if(
                    ( !injectionSet.requiredMod.NullOrEmpty() )&&
                    ( Find_Extensions.ModByName( injectionSet.requiredMod ) == null )
                )
                {
                    continue;
                }
                var thingDefs = DefInjectionQualifier.FilteredThingDefs( injectionSet.qualifier, ref injectionSet.qualifierInt, injectionSet.targetDefs );
                if( !thingDefs.NullOrEmpty() )
                {
#if DEBUG
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append( "InspectTabBases :: Qualifier returned: " );
#endif
                    foreach( var thingDef in thingDefs )
                    {
#if DEBUG
                        stringBuilder.Append( thingDef.defName + ", " );
#endif
                        if( !InjectInspectTabBase( injectionSet.newInspectTabBase, injectionSet.replaceInspectTabBase, thingDef ) )
                        {
                            return false;
                        }
                    }
#if DEBUG
                    CCL_Log.Message( stringBuilder.ToString(), def.ModName );
#endif
                }
            }

            return true;

        }

        private bool                        InjectInspectTabBase( Type newInspectTabBase, Type replaceInspectTabBase, ThingDef thingDef )
        {
            var injectedInspectTabBase = (InspectTabBase) Activator.CreateInstance( newInspectTabBase );
            if( injectedInspectTabBase == null )
            {
                return false;
            }
            if( thingDef.inspectorTabs.NullOrEmpty() )
            {
                thingDef.inspectorTabs = new List<Type>();
            }
            if( thingDef.inspectorTabsResolved.NullOrEmpty() )
            {
                thingDef.inspectorTabsResolved = new List<InspectTabBase>();
            }
            var injectTypeAt = replaceInspectTabBase == null
                ? thingDef.inspectorTabs.Count
                : thingDef.inspectorTabs.IndexOf( replaceInspectTabBase );
            var injectResolvedAt = replaceInspectTabBase == null
                ? thingDef.inspectorTabsResolved.Count
                : thingDef.inspectorTabsResolved.FindIndex( r => r.GetType() == replaceInspectTabBase );
            if( replaceInspectTabBase != null )
            {
                thingDef.inspectorTabs.RemoveAt( injectTypeAt );
                thingDef.inspectorTabsResolved.RemoveAt( injectResolvedAt );
            }
            thingDef.inspectorTabs.Insert( injectTypeAt, newInspectTabBase );
            thingDef.inspectorTabsResolved.Insert( injectResolvedAt, injectedInspectTabBase );
            return true;
        }

    }

}
