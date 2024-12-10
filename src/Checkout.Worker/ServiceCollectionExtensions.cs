using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Checkout.Worker
{
    public static class ServiceCollectionExtensions
    {
        private static IEnumerable<Type> ScanFor(this Assembly assembly, Type assignableType)
        {
            return assembly.GetTypes().Where(t => assignableType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        }
    }
}