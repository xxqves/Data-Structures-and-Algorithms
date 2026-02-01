namespace CustomHashTable
{
    internal struct Entry<TKey, TValue>
    {
        public TKey Key;

        public TValue? Value;

        public bool HasValue;

        public bool IsEmpty => !HasValue;

        public Entry(TKey key, TValue value)
        {
            Key = key;
            Value = value;
            HasValue = true;
        }

        public void Clear()
        {
            Key = default!;
            Value = default;
            HasValue = false;
        }
    }
}
