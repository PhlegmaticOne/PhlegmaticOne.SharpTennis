using System;
using System.Collections.Generic;
using SharpDX;

namespace PhlegmaticOne.SharpTennis.Game.Common
{
    public class Utils
    {
        public static void DisposeDictionaryElements<T>(Dictionary<string, T> dictionary)
            where T: class, IDisposable
        {
            foreach (var disposable in dictionary)
            {
                var element = disposable.Value;
                Utilities.Dispose(ref element);
            }
            dictionary.Clear();
        }

        public static void DisposeListElements<T>(List<T> list) where T : class, IDisposable
        {
            foreach (var disposable in list)
            {
                var d = disposable;
                Utilities.Dispose(ref d);
            }

            list.Clear();
        }
    }
}
