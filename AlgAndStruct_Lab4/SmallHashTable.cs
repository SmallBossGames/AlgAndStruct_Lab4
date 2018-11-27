using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgAndStruct_Lab4
{
    class SmallHashTable<TValue>
    {
        private readonly HashTableNode<string, TValue>[] _nodes;
        private readonly HashType _hashType;

        public SmallHashTable(long capacity, HashType hashType)
        {
            _nodes = new HashTableNode<string, TValue>[capacity];
            _hashType = hashType;
        }

        public void Add(string key, TValue value)
        {
            var hash = GetHash(key);
            
            var current = _nodes[hash];

            while (current != null) 
            {
                if(current.Key==key)
                {
                    throw new ArgumentException("This key are exist");
                }
                current = current.Next;
            }

            _nodes[hash] = new HashTableNode<string, TValue>
            {
                Next = _nodes[hash],
                Key = key,
                Value = value,
            };
        }

        public bool Remove(string key)
        {
            var hash = GetHash(key);

            var current = _nodes[hash];

            if(current == null)
            {
                return false;
            }

            if(current.Key==key)
            {
                _nodes[hash] = current.Next;
                return true;
            }

            while (current.Next!=null)
            {
                var next = current.Next;
                if(next.Key == key)
                {
                    current.Next = next.Next;
                    return true;
                }
                current = next;
            }

            return false;
        }

        public TValue this[string key]
        {
            get
            {
                var success = TryGetNode(key, out var node);
                if(!success)
                {
                    throw new KeyNotFoundException();
                }
                return node.Value;
            }
            set
            {
                var success = TryGetNode(key, out var node);
                if (!success)
                {
                    Add(key, value);
                    return;
                }
                node.Value = value;
            }
        }

        public bool TryGetValue(string key, out TValue value)
        {
            var success = TryGetNode(key, out var node);
            value = node.Value;
            return success;
        }

        private bool TryGetNode(string key, out HashTableNode<string, TValue> tableNode)
        {
            var hash = GetHash(key);

            var current = _nodes[hash];

            while (current != null)
            {
                if (current.Key == key)
                {
                    tableNode = current;
                    return true;
                }
                current = current.Next;
            }

            tableNode = null;
            return false;
        }

        private long GetHash(string key)
        {
            switch (_hashType)
            {
                case HashType.ConcatHash:
                    return GetConcatHash(key);
                case HashType.AdaptiveHash:
                    return GetAdaptiveHash(key);
                default:
                    throw new ArgumentException();
            }
        }

        private long GetConcatHash(string key)
        {
            var temp = 0L;
            for (int i = 0; i < key.Length; i++)
            {
                temp = (temp << 4) | key[i];
            }
            return temp % _nodes.LongLength;
        }

        private long GetAdaptiveHash(string key)
        {
            var temp = 0L;
            for (int i = 0; i < key.Length; i++)
            {
                temp += key[i];
            }
            return temp % _nodes.LongLength;
        }
    }

    class HashTableNode<TKey,TValue>
    {
        public HashTableNode<TKey, TValue> Next { get; set; }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    }

    enum HashType
    {
        ConcatHash,
        AdaptiveHash,
    }
}
