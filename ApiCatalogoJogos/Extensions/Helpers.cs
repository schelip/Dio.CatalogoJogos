using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ApiCatalogoJogos.Extensions
{
    public static class Helpers
    {
        public static IEnumerable<Type> GetTypes(string nameSpace, Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.Namespace == nameSpace);
        }
    }
}
