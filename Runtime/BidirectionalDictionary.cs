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
        readonly Dictionary<KEY, VALUE> keyToValueMap;
        readonly Dictionary<VALUE, KEY> valueToKeyMap;

        public VALUE this[KEY key]
        {
            get => keyToValueMap[key];
            set => keyToValueMap[key] = value;
        }

        public ICollection<KEY> Keys => ((IDictionary<KEY, VALUE>)keyToValueMap).Keys;

        public ICollection<VALUE> Values => ((IDictionary<KEY, VALUE>)keyToValueMap).Values;

        public int Count => keyToValueMap.Count;

        public bool IsReadOnly => ((IDictionary<KEY, VALUE>)keyToValueMap).IsReadOnly;

        public void Add(KEY key, VALUE value)
        {
            keyToValueMap.Add(key, value);
        }

        public void Add(KeyValuePair<KEY, VALUE> item)
        {
            ((IDictionary<KEY, VALUE>)keyToValueMap).Add(item);
        }

        public void Clear()
        {
            keyToValueMap.Clear();
        }

        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            return ((IDictionary<KEY, VALUE>)keyToValueMap).Contains(item);
        }

        public bool ContainsKey(KEY key)
        {
            return keyToValueMap.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<KEY, VALUE>[] array, int arrayIndex)
        {
            ((IDictionary<KEY, VALUE>)keyToValueMap).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return ((IDictionary<KEY, VALUE>)keyToValueMap).GetEnumerator();
        }

        public bool Remove(KEY key)
        {
            return keyToValueMap.Remove(key);
        }

        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            return ((IDictionary<KEY, VALUE>)keyToValueMap).Remove(item);
        }

        public bool TryGetValue(KEY key, out VALUE value)
        {
            return keyToValueMap.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<KEY, VALUE>)keyToValueMap).GetEnumerator();
        }
    }
}
