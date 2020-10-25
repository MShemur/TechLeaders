using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DictionarySpace
{
    public static class Extensions
    {
        public static int GetHash(this string str)
        {
            return str.GetHashCode();
            //for testing
            unchecked
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                for (int i = 0; i < str.Length; i += 2)
                {
                    hash1 = ((hash1 << 5) + hash1) ^ str[i];
                    if (i == str.Length - 1)
                        break;
                    hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
                }

                return hash1 + (hash2 * 1566083941);
            }
        }
    }

    public class Dictionary<TKey, TVal> : IDictionary<TKey, TVal>
    {
        public ICollection<TKey> Keys { get; }
        public ICollection<TVal> Values { get; }

        public int Count => count;

        public bool IsReadOnly { get; } = false;

        private int[] buckets;
        private Entry[] entries;

        private int capacity;

        private int indexEntries;
        private int count;
        private int freeList = -1;

        public Dictionary()
        {
            capacity = 4;
            buckets = GetNewBuckets(capacity);
            entries = GetNewEntries(capacity);
            Keys = new List<TKey>();
            Values = new List<TVal>();
        }

        private int[] GetNewBuckets(int capacity)
        {
            int[] buckets = new int[capacity];
            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = -1;
            }

            return buckets;
        }

        private Entry[] GetNewEntries(int capacity)
        {
            Entry[] entries = new Entry[capacity];
            for (int i = 0; i < entries.Length; i++)
            {
                entries[i] = new Entry() { next = -1 };
            }

            return entries;
        }


        public void Add(KeyValuePair<TKey, TVal> item)
        {
            Add(item.Key, item.Value);
        }

        public IEnumerator<KeyValuePair<TKey, TVal>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Clear()
        {
            for (int i = 0; i < Keys.Count;)
            {
                Remove(Keys.ElementAt(i));
            }
        }

        public bool Contains(KeyValuePair<TKey, TVal> item)
        {
            TryGetValue(item.Key, out var value);
            return item.Value.Equals(value);
        }

        public void CopyTo(KeyValuePair<TKey, TVal>[] array, int arrayIndex)
        {
            for (int i = 0; i < Keys.Count; i++)
            {
                var key = Keys.ElementAt(i);
                TryGetValue(key, out var value);
                array[i + arrayIndex] = new KeyValuePair<TKey, TVal>(key, value);
            }
        }

        public bool Remove(KeyValuePair<TKey, TVal> item)
        {
            return Remove(item.Key);
        }


        public void Add(TKey key, TVal value)
        {
            if (count >= capacity && freeList == -1) Resize();
            int hashCode;
            if (key is string) hashCode = (key as string).GetHash();
            else hashCode = key.GetHashCode();
            int bucketNum = (hashCode & 0x7fffffff) % capacity;

            var index = freeList == -1 ? indexEntries : freeList;

            freeList = freeList == -1 ? -1 : entries[index].next;

            int prevIndex = buckets[bucketNum];
            buckets[bucketNum] = index;
            if (prevIndex != -1 && entries[prevIndex].hashCode == hashCode && key.Equals(entries[prevIndex].key))
            {
                throw new Exception("Duplicate key detected");
            }

            entries[index] = new Entry(hashCode, key, value) { next = prevIndex };
            Keys.Add(key);
            Values.Add(value);


            if (freeList == -1)
            {
                count++;
                indexEntries++;
            }
        }

        private void Resize()
        {
            var oldCapacity = capacity;
            capacity *= 2;

            int[] newBuckets = GetNewBuckets(capacity);
            Entry[] newEntries = GetNewEntries(capacity);

            Array.Copy(entries, newEntries, buckets.Length);

            for (int i = 0; i < count; i++)
            {
                int hashCode = newEntries[i].hashCode;
                ;
                if (hashCode == 0)
                {
                    continue;
                }

                int bucketNum = (hashCode & 0x7fffffff) % capacity;
                newEntries[i].next = newBuckets[bucketNum];
                newBuckets[bucketNum] = i;
            }

            buckets = newBuckets;
            entries = newEntries;
        }

        public bool ContainsKey(TKey key)
        {
            return Keys.Contains(key);
        }

        public bool Remove(TKey key)
        {
            int hashCode;
            if (key is string) hashCode = (key as string).GetHash();
            else hashCode = key.GetHashCode();
            int bucketNum = (hashCode & 0x7fffffff) % capacity;

            int i = buckets[bucketNum];
            int previous = -1;

            while (i >= 0)
            {
                if (entries[i].hashCode == hashCode && key.Equals(entries[i].key))
                {
                    if (previous == -1)
                    {
                        if (entries[i].next != -1) buckets[bucketNum] = entries[i].next;
                        else buckets[bucketNum] = -1;
                    }
                    else
                    {
                        entries[previous].next = entries[i].next;
                    }

                    Keys.Remove(key);
                    Values.Remove(entries[i].value);
                    entries[i] = new Entry() { next = freeList };
                    freeList = i;
                    return true;
                }

                previous = i;
                i = entries[i].next;
            }

            return false;
        }

        public bool TryGetValue(TKey key, out TVal value)
        {
            int hashCode;
            if (key is string) hashCode = (key as string).GetHash();
            else hashCode = key.GetHashCode();
            int bucketNum = (hashCode & 0x7fffffff) % capacity;

            int i = buckets[bucketNum];

            while (i >= 0)
            {
                if (entries[i].hashCode == hashCode && key.Equals(entries[i].key))
                {
                    value = entries[i].value;
                    return true;
                }

                i = entries[i].next;
            }

            value = default(TVal);
            return false;
        }

        public TVal this[TKey key]
        {
            get
            {
                TryGetValue(key, out var value);
                return value;
            }
            set => Add(key, value);
        }

        internal struct Entry
        {
            public int hashCode;
            public TKey key;
            public TVal value;
            public int next;

            public Entry(int hashCode, TKey key, TVal value) : this()
            {
                this.hashCode = hashCode;
                this.key = key;
                this.value = value;
                next = -1;
            }
        }
    }
}