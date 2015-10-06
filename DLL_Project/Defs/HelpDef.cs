﻿using System;

using Verse;

namespace CommunityCoreLibrary
{

    public class HelpDef : Def, IComparable
    {

        #region XML Data

        public HelpCategoryDef category;

        #endregion

        [Unsaved]

        #region Instance Data

        public string                   keyDef;

        #endregion

        #region Process State

#if DEBUG
        public override void ResolveReferences()
        {
            base.ResolveReferences();
            if( category == null )
            {
                CCL_Log.Error( "Category resolved to null", defName );
            }
        }
#endif

        public int CompareTo(object obj)
        {
            var d = obj as HelpDef;
            return d != null
                ? d.label.CompareTo(label) * -1
                : 1;
        }

        #endregion

        #region Log Dump

        public string LogDump()
        {
            return 
                "HelpDef: " + defName +
                "\n\t" + keyDef +
                "\n\t" + category.LabelCap +
                "\n\t" + LabelCap +
                "\n------\n" +
                description +
                "\n------\n";
        }

        #endregion

    }

}
