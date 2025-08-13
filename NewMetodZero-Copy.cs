using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class HashSetExtensions
{
    public static IReadOnlyList<T> ConvertToList<T>(this HashSet<T> hashSet)
    {
        if (hashSet == null)
            throw new ArgumentNullException(nameof(hashSet));
        
        return new ImmutableListView<T>(hashSet);
    }

    private sealed class ImmutableListView<T> : IReadOnlyList<T>
    {
        private readonly HashSet<T> _sourceSet;
        private List<T>? _cachedList;

        public ImmutableListView(HashSet<T> sourceSet)
        {
            _sourceSet = sourceSet ?? throw new ArgumentNullException(nameof(sourceSet));
        }

        public T this[int index]
        {
            get
            {
                // Автоматическое кеширование при первом обращении по индексу
                if (_cachedList == null)
                {
                    _cachedList = new List<T>(_sourceSet);
                }
                return _cachedList[index];
            }
        }

        public int Count => _sourceSet.Count;

        public IEnumerator<T> GetEnumerator() => _sourceSet.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
