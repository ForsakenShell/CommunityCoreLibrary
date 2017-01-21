using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using RimWorld;
using Verse;
using UnityEngine;

namespace CommunityCoreLibrary.Detour
{
    
    internal static class _PlaySettings
    {
        internal static FieldInfo           _showColonistBar;
        internal static FieldInfo           _lockNorthUp;
        internal static FieldInfo           _usePlanetDayNightSystem;
        internal static FieldInfo           _expandingIcons;

        internal static List<ToggleSettingDef>  toggleSettingDefs;

        static                              _PlaySettings()
        {
            _showColonistBar = typeof( PlaySettings ).GetField( "showColonistBar", Controller.Data.UniversalBindingFlags );
            if ( _showColonistBar == null )
            {
                CCL_Log.Trace(
                    Verbosity.FatalErrors,
                    "Unable to get field 'showColonistBar' in 'PlaySettings'",
                    "Detour.PlaySettings" );
            }

            _lockNorthUp = typeof( PlaySettings ).GetField( "lockNorthUp", Controller.Data.UniversalBindingFlags );
            if ( _lockNorthUp == null )
            {
                CCL_Log.Trace(
                    Verbosity.FatalErrors,
                    "Unable to get field 'lockNorthUp' in 'PlaySettings'",
                    "Detour.PlaySettings" );
            }

            _usePlanetDayNightSystem = typeof(PlaySettings).GetField("usePlanetDayNightSystem", Controller.Data.UniversalBindingFlags);
            if (_usePlanetDayNightSystem == null)
            {
                CCL_Log.Trace(
                    Verbosity.FatalErrors,
                    "Unable to get field 'usePlanetDayNightSystem' in 'PlaySettings'",
                    "Detour.PlaySettings");
            }

            _expandingIcons = typeof(PlaySettings).GetField("expandingIcons", Controller.Data.UniversalBindingFlags);
            if (_expandingIcons == null)
            {
                CCL_Log.Trace(
                    Verbosity.FatalErrors,
                    "Unable to get field 'expandingIcons' in 'PlaySettings'",
                    "Detour.PlaySettings");
            }

            toggleSettingDefs = DefDatabase<ToggleSettingDef>.AllDefs.ToList();
            toggleSettingDefs.Sort( (x, y) => x.order > y.order ? 1 : -1 );
        }
        
        #region Reflected Methods

        internal static bool ShowColonistBar
        {
            get
            {
                return (bool)_showColonistBar.GetValue(null);
            }

            set
            {
                _showColonistBar.SetValue(null, value);
            }
        }

        internal static bool LockNorthUp
        {
            get
            {
                return (bool)_lockNorthUp.GetValue( null );
            }

            set
            {
                _lockNorthUp.SetValue( null, value );
            }
        }

        internal static bool UsePlanetDayNightSystem
        {
            get
            {
                return (bool)_usePlanetDayNightSystem.GetValue(null);
            }

            set
            {
                _usePlanetDayNightSystem.SetValue(null, value);
            }
        }

        internal static bool ExpandingIcons
        {
            get
            {
                return (bool)_expandingIcons.GetValue(null);
            }

            set
            {
                _expandingIcons.SetValue(null, value);
            }
        }

        #endregion

        // TODO: update toggle setting list: lockNorthUp, usePlanetDayNightSystem, expandingIcons
        [DetourMember( typeof( PlaySettings ) )]
        internal static void                _ExposeData( this PlaySettings _this )
        {
            foreach( var toggleSetting in toggleSettingDefs )
            {
                if( toggleSetting.exposeValue )
                {
                    bool value = toggleSetting.toggleWorker.Value;
                    Scribe_Values.LookValue( ref value, toggleSetting.saveKey, false, false );

                    if( Scribe.mode == LoadSaveMode.LoadingVars )
                    {
                        toggleSetting.toggleWorker.Value = value;
                    }
                }
            }
        }

        [DetourMember( typeof( PlaySettings ) )]
        internal static void                _DoPlaySettingsGlobalControls( this PlaySettings _this, WidgetRow row, bool worldView )
        {
            bool oldShowColonistBar = ShowColonistBar;
            if( worldView )
            {
                /* TODO
                if( Current.ProgramState == ProgramState.Playing)
                {
                    bool newShowColonistBar = ShowColonistBar;
                    row.ToggleableIcon(ref newShowColonistBar, TexButton.ShowColonistBar, "ShowColonistBarToggleButton".Translate(), SoundDefOf.MouseoverToggle, null);
                    ShowColonistBar = newShowColonistBar;
                }
                bool oldLockNorthUp = LockNorthUp;
                bool newLockNorthUp = LockNorthUp;
                row.ToggleableIcon(ref newLockNorthUp, TexButton.LockNorthUp, "LockNorthUpToggleButton".Translate(), SoundDefOf.MouseoverToggle, null);
                LockNorthUp = newLockNorthUp;
                if (oldLockNorthUp != newLockNorthUp && LockNorthUp)
                {
                    Find.WorldCameraDriver.RotateSoNorthIsUp(true);
                }
                if (Current.ProgramState == ProgramState.Playing)
                {
                    bool newUsePlanetDayNightSystem = UsePlanetDayNightSystem;
                    row.ToggleableIcon(ref newUsePlanetDayNightSystem, TexButton.UsePlanetDayNightSystem, "UsePlanetDayNightSystemToggleButton".Translate(), SoundDefOf.MouseoverToggle, null);
                    UsePlanetDayNightSystem = newUsePlanetDayNightSystem;
                }
                bool newExpandingIcons = ExpandingIcons;
                row.ToggleableIcon(ref newExpandingIcons, TexButton.ExpandingIcons, "ExpandingIconsToggleButton".Translate(), SoundDefOf.MouseoverToggle, null);
                ExpandingIcons = newExpandingIcons;
                */
            }
            else
            {
                foreach( var toggleSetting in toggleSettingDefs )
                {
                    if( toggleSetting.enableButton )
                    {
                        bool value = toggleSetting.toggleWorker.Value;

                        row.ToggleableIcon( ref value, toggleSetting.icon, toggleSetting.Label, toggleSetting.soundDef, toggleSetting.tutorTag );

                        if( value == toggleSetting.toggleWorker.Value )
                        {
                            continue;
                        }

                        toggleSetting.toggleWorker.Value = value;
                    }
                }
            }

            if( oldShowColonistBar != ShowColonistBar)
            {
                Find.ColonistBar.MarkColonistsDirty();
            }
        }

    }

}
