using System;
using System.Collections.Generic;
using System.Reflection;

namespace Discuz.Common.TypeFinder
{
    public interface ITypeFinder
    {
        IList<Assembly> GetFilteredAssemblyList();
    }
}
