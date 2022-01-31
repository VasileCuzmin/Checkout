using System.Collections;
using System.Collections.Generic;

namespace Checkout.Domain.BasketAggregate
{
    public interface IReadonlyHashSet<T> : IReadOnlyCollection<T>
    {
        public bool Contains(T i);
    }

    public class ReadonlyHashSet<T> : IReadonlyHashSet<T>
    {
        public int Count => set.Count;
        private HashSet<T> set;

        public ReadonlyHashSet(HashSet<T> set) => this.set = set;
        public bool Contains(T i) => set.Contains(i);
        public IEnumerator<T> GetEnumerator() => set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => set.GetEnumerator();
    }

    public static class HasSetExtensions
    {
        public static ReadonlyHashSet<T> AsReadOnly<T>(this HashSet<T> s)
            => new ReadonlyHashSet<T>(s);
    }
}