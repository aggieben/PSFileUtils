using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSFileUtils.FileStat
{
    class Comparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparer;

        public Comparer(Func<T, T, bool> comparer)
        {
            _comparer = comparer;
        }

        public override bool Equals(T x, T y)
        {
            return _comparer(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }
}
