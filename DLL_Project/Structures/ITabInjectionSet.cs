using System;
using System.Collections.Generic;

namespace CommunityCoreLibrary
{
    
    public struct InspectTabBaseInjectionSet
    {

        public string                       requiredMod;

        public Type                         newInspectTabBase;
        public Type                         replaceInspectTabBase;

        public List< string >               targetDefs;

        public Type                         qualifier;

        public DefInjectionQualifier        qualifierInt;

    }

}

