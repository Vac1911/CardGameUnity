using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using System.Linq;

namespace Tools
{
    public static class Utils
    {
        public static Type[] GetInheritedClasses(Type MyType)
        {
            return Assembly.GetAssembly(MyType).GetTypes().Where(TheType => TheType.IsClass && !TheType.IsAbstract && TheType.IsSubclassOf(MyType)).ToArray();
        }

        public static Type[] GetImplementingClasses(Type MyType)
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes()).Where(p => MyType.IsAssignableFrom(p) && !p.IsInterface).ToArray();
        }
    }
}
