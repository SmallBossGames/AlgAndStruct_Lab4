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

#if DEBUG

        public readonly int[] collisions;

#endif


        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="capacity">Объём массива узлов</param>
        /// <param name="hashType">Алгоритм хеширования</param>
        public SmallHashTable(long capacity, HashType hashType)
        {
            _nodes = new HashTableNode<string, TValue>[capacity];
            _hashType = hashType;

#if DEBUG
            collisions = new int[capacity];

            for (int i = 0; i < capacity; i++)
            {
                collisions[i] = 0;
            }
#endif
        }

        /// <summary>
        /// Добавить элемент в таблицу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Значение</param>
        public void Add(string key, TValue value)
        {
            var hash = GetHash(key);

            var current = _nodes[hash];

#if DEBUG
            if(_nodes[hash] != null)
            {
                collisions[hash]++;
            }
#endif

            while (current != null) 
            {
                if (current.Key == key)
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

        /// <summary>
        /// Удаление элемента из таблицы
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            var hash = GetHash(key);

            var current = _nodes[hash];

            if (current == null)
            {
                return false;
            }

            if (current.Key == key)
            {
                _nodes[hash] = current.Next;
                return true;
            }

            while (current.Next != null)
            {
                var next = current.Next;
                if (next.Key == key)
                {
                    current.Next = next.Next;
                    return true;
                }
                current = next;
            }

            return false;
        }

        /// <summary>
        /// Индексатор, для произвольного доступа к элементам
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public TValue this[string key]
        {
            get
            {
                var success = TryGetNode(key, out var node);
                if (!success)
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

        /// <summary>
        /// Получение значения конкретного узла
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Выходное значение</param>
        /// <returns></returns>
        public bool TryGetValue(string key, out TValue value)
        {
            var success = TryGetNode(key, out var node);
            value = node.Value;
            return success;
        }

        /// <summary>
        /// Получение объекта конкретного узла
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <param name="value">Узел</param>
        /// <returns></returns>
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

        /// <summary>
        /// Получить хеш-код
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        private long GetHash(string key)
        {
            switch (_hashType)
            {
                case HashType.ConcatHash:
                    return GetConcatHash(key);
                case HashType.AdaptiveHash:
                    return GetAdditiveHash(key);
                default:
                    throw new ArgumentException();
            }
        }

        /// <summary>
        /// Получить хеш-код алгоритмом конкатинации
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        private long GetConcatHash(string key)
        {
            var temp = 0L;
            for (int i = 0; i < key.Length; i++)
            {
                temp = (temp << 8) | key[i];
            }
            return temp % _nodes.LongLength;
        }

        /// <summary>
        /// Получить хеш-код аддитивным алгоритмом
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        private long GetAdditiveHash(string key)
        {
            var temp = 0L;
            for (int i = 0; i < key.Length; i++)
            {
                temp += key[i];
            }
            return temp % _nodes.LongLength;
        }
    }

    class HashTableNode<TKey, TValue>
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