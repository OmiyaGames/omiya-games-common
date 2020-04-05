using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <copyright file="BidirectionalDictionary.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020-2020 Omiya Games
    /// 
    /// Permission is hereby granted, free of charge, to any person obtaining a copy
    /// of this software and associated documentation files (the "Software"), to deal
    /// in the Software without restriction, including without limitation the rights
    /// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    /// copies of the Software, and to permit persons to whom the Software is
    /// furnished to do so, subject to the following conditions:
    /// 
    /// The above copyright notice and this permission notice shall be included in
    /// all copies or substantial portions of the Software.
    /// 
    /// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    /// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    /// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    /// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    /// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    /// THE SOFTWARE.
    /// </copyright>
    /// <author>Taro Omiya</author>
    /// <date>4/4/2020</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A two-direction dictionary where a key maps to a value, and vice versa.
    /// </summary>
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    ///   <listheader>
    ///     <description>Date</description>
    ///     <description>Name</description>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <description>4/4/2020</description>
    ///     <description>Taro</description>
    ///     <description>Initial version</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public class BidirectionalDictionary<KEY, VALUE> : IDictionary<KEY, VALUE>
    {
        /// <summary>
        /// The dictionary mapping from key to value.
        /// </summary>
        /// <seealso cref="ValueToKeyMap"/>
        private Dictionary<KEY, VALUE> KeyToValueMap
        {
            get;
        }
        /// <summary>
        /// The dictionary mapping from value to key.
        /// </summary>
        /// <seealso cref="KeyToValueMap"/>
        private Dictionary<VALUE, KEY> ValueToKeyMap
        {
            get;
        }

        #region Constructors
        public BidirectionalDictionary()
        {
            KeyToValueMap = new Dictionary<KEY, VALUE>();
            ValueToKeyMap = new Dictionary<VALUE, KEY>();
        }

        public BidirectionalDictionary(int capacity)
        {
            KeyToValueMap = new Dictionary<KEY, VALUE>(capacity);
            ValueToKeyMap = new Dictionary<VALUE, KEY>(capacity);
        }

        public BidirectionalDictionary(IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer)
        {
            KeyToValueMap = new Dictionary<KEY, VALUE>(keyComparer);
            ValueToKeyMap = new Dictionary<VALUE, KEY>(valueComparer);
        }

        public BidirectionalDictionary(int capacity, IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer)
        {
            KeyToValueMap = new Dictionary<KEY, VALUE>(capacity, keyComparer);
            ValueToKeyMap = new Dictionary<VALUE, KEY>(capacity, valueComparer);
        }

        public BidirectionalDictionary(IDictionary<KEY, VALUE> dictionary) : this()
        {
            foreach(KeyValuePair<KEY, VALUE> pair in dictionary)
            {
                Add(pair);
            }
        }

        public BidirectionalDictionary(IDictionary<KEY, VALUE> dictionary, IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer) : this(keyComparer, valueComparer)
        {
            foreach (KeyValuePair<KEY, VALUE> pair in dictionary)
            {
                Add(pair);
            }
        }
        #endregion

        public VALUE this[KEY key]
        {
            get => KeyToValueMap[key];
            set => KeyToValueMap[key] = value;
        }

        public ICollection<KEY> Keys => KeyToValueMap.Keys;

        public ICollection<VALUE> Values => KeyToValueMap.Values;

        public int Count => KeyToValueMap.Count;

        public bool IsReadOnly => ((IDictionary<KEY, VALUE>)KeyToValueMap).IsReadOnly;

        public void Add(KEY key, VALUE value)
        {
            KeyToValueMap.Add(key, value);
        }

        public void Add(KeyValuePair<KEY, VALUE> item)
        {
            ((IDictionary<KEY, VALUE>)KeyToValueMap).Add(item);
        }

        public void Clear()
        {
            KeyToValueMap.Clear();
        }

        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return ((IDictionary<KEY, VALUE>)KeyToValueMap).Contains(item);
        }

        public bool ContainsKey(KEY key)
        {
            return KeyToValueMap.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<KEY, VALUE>[] array, int arrayIndex)
        {
            ((IDictionary<KEY, VALUE>)KeyToValueMap).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return ((IDictionary<KEY, VALUE>)KeyToValueMap).GetEnumerator();
        }

        public bool Remove(KEY key)
        {
            return KeyToValueMap.Remove(key);
        }

        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            return ((IDictionary<KEY, VALUE>)KeyToValueMap).Remove(item);
        }

        public bool TryGetValue(KEY key, out VALUE value)
        {
            return KeyToValueMap.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<KEY, VALUE>)KeyToValueMap).GetEnumerator();
        }
    }
}
