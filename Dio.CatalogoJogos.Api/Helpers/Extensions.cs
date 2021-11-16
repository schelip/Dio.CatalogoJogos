using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Dio.CatalogoJogos.Api.Helpers
{
    public static class Extensions
    {
        public static IEnumerable<Type> GetTypes(this Assembly assembly, string nameSpace)
        {
            return assembly.GetTypes().Where(t => t.Namespace == nameSpace);
        }
    }
}
