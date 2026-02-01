namespace CustomHashTable
{
    internal class HashTable<TKey, TValue>
    {
        private List<Entry<TKey, TValue>>[] _buckets;

        private const double LOAD_FACTOR = 0.7;

        private int _count;

        public int Count => _count;

        public HashTable(int capacity = 16)
        {
            InitializeBuckets(capacity);
        }

        public void Add(TKey key, TValue value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            int index = GetHashIndex(key, _buckets.Length);
            var bucket = _buckets[index];

            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    throw new ArgumentException($"An item with the same key has already been added. Key: '{key}'");
                }
            }

            if (_count >= _buckets.Length * LOAD_FACTOR)
            {
                Resize();
                index = GetHashIndex(key, _buckets.Length);
                bucket = _buckets[index];
            }

            bucket.Add(new Entry<TKey, TValue>(key, value));
            _count++;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            int index = GetHashIndex(key, _buckets.Length);
            var bucket = _buckets[index];
            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    value = bucket[i].Value;
                    return true;
                }
            }

            value = default;
            return false;
        }

        public bool ContainsKey(TKey key)
        {
            return TryGetValue(key, out _);
        }

        public bool Remove(TKey key)
        {
            if (key is null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            int index = GetHashIndex(key, _buckets.Length);
            var bucket = _buckets[index];

            for (int i = 0; i < bucket.Count; i++)
            {
                if (bucket[i].Key.Equals(key))
                {
                    bucket.RemoveAt(i);
                    _count--;
                    return true;
                }
            }

            return false;
        }

        public void Clear()
        {
            if (_count == 0)
            {
                return;
            }

            for (int i = 0; i < _buckets.Length; i++)
            {
                _buckets[i].Clear();
            }
            _count = 0;
        }

        public TValue this[TKey key]
        {
            get
            {
                if (TryGetValue(key, out TValue value))
                {
                    return value;
                }
                throw new KeyNotFoundException();
            }
            set
            {
                if (key is null)
                {
                    throw new ArgumentNullException(nameof(key));
                }

                int index = GetHashIndex(key, _buckets.Length);
                var bucket = _buckets[index];

                for (int i = 0; i < bucket.Count; i++)
                {
                    if (bucket[i].Key.Equals(key))
                    {
                        var entry = bucket[i];
                        entry.Value = value;
                        bucket[i] = entry;
                        return;
                    }
                }

                if (_count >= _buckets.Length * LOAD_FACTOR)
                {
                    Resize();
                    index = GetHashIndex(key, _buckets.Length);
                    bucket = _buckets[index];
                }

                bucket.Add(new Entry<TKey, TValue>(key, value));
                _count++;
            }
        }

        private void InitializeBuckets(int capacity)
        {
            _buckets = new List<Entry<TKey, TValue>>[capacity];
            for (int i = 0; i < capacity; i++)
            {
                _buckets[i] = new List<Entry<TKey, TValue>>();
            }
        }

        private void Resize()
        {
            int newCapacity = _buckets.Length * 2;
            List<Entry<TKey, TValue>>[] newBuckets = new List<Entry<TKey, TValue>>[newCapacity];

            for (int i = 0; i < newCapacity; i++)
            {
                newBuckets[i] = new List<Entry<TKey, TValue>>();
            }

            for (int i = 0; i < _buckets.Length; i++)
            {
                var oldBucket = _buckets[i];
                foreach (var entry in oldBucket)
                {
                    int newIndex = GetHashIndex(entry.Key, newCapacity);
                    newBuckets[newIndex].Add(entry);
                }
            }

            _buckets = newBuckets;
        }

        private int GetHashIndex(TKey key, int capacity)
        {
            int hash = key.GetHashCode() & 0x7FFFFFFF;
            return hash % capacity;
        }
    }
}
