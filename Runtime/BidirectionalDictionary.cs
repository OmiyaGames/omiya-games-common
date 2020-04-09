using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;

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
    /// This does mean both keys and values must be unique.
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

        public BidirectionalDictionary(IDictionary<KEY, VALUE> dictionary) : this(dictionary.Count)
        {
            foreach (KeyValuePair<KEY, VALUE> pair in dictionary)
            {
                Add(pair.Key, pair.Value);
            }
        }

        public BidirectionalDictionary(IDictionary<KEY, VALUE> dictionary, IEqualityComparer<KEY> keyComparer, IEqualityComparer<VALUE> valueComparer) : this(dictionary.Count, keyComparer, valueComparer)
        {
            foreach (KeyValuePair<KEY, VALUE> pair in dictionary)
            {
                Add(pair.Key, pair.Value);
            }
        }
        #endregion

        /// <summary>
        /// The comparer used to check whether two keys equal each other.
        /// </summary>
        public IEqualityComparer<KEY> KeyComparer => KeyToValueMap.Comparer;

        /// <summary>
        /// The comparer used to check whether two values equal each other.
        /// </summary>
        public IEqualityComparer<VALUE> ValueComparer => ValueToKeyMap.Comparer;

        #region Implemented Properties
        /// <summary>
        /// Same as <see cref="GetValue(KEY)"/> and <see cref="SetKey(VALUE, KEY)"/>:
        /// gets and sets a corresponding value.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public VALUE this[KEY key]
        {
            get => GetValue(key);
            set => SetValue(key, value);
        }

        public ICollection<KEY> Keys => KeyToValueMap.Keys;

        public ICollection<VALUE> Values => ValueToKeyMap.Keys;

        public int Count => KeyToValueMap.Count;

        public bool IsReadOnly => ((IDictionary<KEY, VALUE>)KeyToValueMap).IsReadOnly;
        #endregion

        /// <summary>
        /// Adds a new key and value combo, if and only if the key and the values are both unique.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(KEY key, VALUE value)
        {
            // Make sure the key and the value isn't already in their respective dictionaries.
            if ((ContainsKey(key) == false) && (ContainsValue(value) == false))
            {
                KeyToValueMap.Add(key, value);
                ValueToKeyMap.Add(value, key);
            }
        }

        /// <summary>
        /// Gets a corresponding value that's paired with a key.
        /// </summary>
        public VALUE GetValue(KEY key)
        {
            return KeyToValueMap[key];
        }

        /// <summary>
        /// Gets a corresponding key that's paired with a value.
        /// </summary>
        public KEY GetKey(VALUE value)
        {
            return ValueToKeyMap[value];
        }

        /// <summary>
        /// Checks if a key exists, and if so, returns the corresponding value.
        /// </summary>
        public bool TryGetValue(KEY key, out VALUE value)
        {
            return KeyToValueMap.TryGetValue(key, out value);
        }

        /// <summary>
        /// Checks if a value exists, and if so, returns the corresponding key.
        /// </summary>
        public bool TryGetKey(VALUE value, out KEY key)
        {
            return ValueToKeyMap.TryGetValue(value, out key);
        }

        /// <summary>
        /// Checks if a key exists, and the newValue doesn't;
        /// if so, replaces the key's pairing to newValue.
        /// </summary>
        public bool SetValue(KEY key, VALUE newValue)
        {
            // First make sure the key is already in the dictionary, AND newValue isn't
            bool returnFlag = false;
            if ((ContainsKey(key) == true) && (ContainsValue(newValue) == false))
            {
                // If so, first remove the old value from the value map
                ValueToKeyMap.Remove(KeyToValueMap[key]);

                // Remap the key to newValue
                KeyToValueMap[key] = newValue;

                // Add the new key to the value map
                ValueToKeyMap.Add(newValue, key);

                // Return true
                returnFlag = true;
            }
            return returnFlag;
        }

        /// <summary>
        /// Checks if a value exists, and if so, returns the corresponding key.
        /// </summary>
        public bool SetKey(VALUE value, KEY newKey)
        {
            // First make sure the key is already in the dictionary, AND newKey isn't
            bool returnFlag = false;
            if ((ContainsValue(value) == true) && (ContainsKey(newKey) == false))
            {
                // If so, first remove the old key from the key map
                KeyToValueMap.Remove(ValueToKeyMap[value]);

                // Remap the value to newKey
                ValueToKeyMap[value] = newKey;

                // Add the new value to the key map
                KeyToValueMap.Add(newKey, value);

                // Return true
                returnFlag = true;
            }
            return returnFlag;
        }

        public bool RemoveKey(KEY key)
        {
            // Check if the key exists
            bool returnFlag = false;
            if (ContainsKey(key) == true)
            {
                // If so, remove from the value map first (this is to keep the mapping)
                ValueToKeyMap.Remove(KeyToValueMap[key]);

                // Then remove from the key map
                KeyToValueMap.Remove(key);
                returnFlag = true;
            }
            return returnFlag;
        }

        public bool RemoveValue(VALUE value)
        {
            // Check if the key exists
            bool returnFlag = false;
            if (ContainsValue(value) == true)
            {
                // If so, remove from the key map first (this is to keep the mapping)
                KeyToValueMap.Remove(ValueToKeyMap[value]);

                // Then remove from the value map
                ValueToKeyMap.Remove(value);
                returnFlag = true;
            }
            return returnFlag;
        }

        public bool ContainsKey(KEY key)
        {
            return KeyToValueMap.ContainsKey(key);
        }

        public bool ContainsValue(VALUE value)
        {
            return ValueToKeyMap.ContainsKey(value);
        }

        #region Implemented Methods
        public void Clear()
        {
            KeyToValueMap.Clear();
            ValueToKeyMap.Clear();
        }

        public void Add(KeyValuePair<KEY, VALUE> item)
        {
            Add(item.Key, item.Value);
        }

        /// <summary>
        /// Same as <see cref="RemoveKey(KEY)"/>: removes a key and its corresponding value from the dictionary.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <seealso cref="RemoveKey(KEY)"/>
        public bool Remove(KEY key)
        {
            return RemoveKey(key);
        }

        /// <summary>
        /// Removes a key-value pair if and only if there's a corresponding key-value map.
        /// </summary>
        /// <param name="item">Pairing to remove.</param>
        /// <returns>True if successfully removed; false, otherwise.</returns>
        /// <seealso cref="Contains(KeyValuePair{KEY, VALUE})"/>
        public bool Remove(KeyValuePair<KEY, VALUE> item)
        {
            // Check if the key exists, then check if the argument's value matches with the VALUE mapped in KeyToValueMap
            bool returnFlag = false;
            if (Contains(item) == true)
            {
                // If so, remove the key and value accordingly.
                KeyToValueMap.Remove(item.Key);
                ValueToKeyMap.Remove(item.Value);
                returnFlag = true;
            }
            return returnFlag;
        }

        /// <summary>
        /// Checks if this dictionary contains the key-to-value pairing.
        /// </summary>
        /// <param name="item">Pairing to verify whether this dictionary contains or not.</param>
        /// <returns>True, if and only if this dictionary contains both has the same key and value that is paired to that key.</returns>
        public bool Contains(KeyValuePair<KEY, VALUE> item)
        {
            // Check if the key exists, then check if the argument's value matches with the VALUE mapped in KeyToValueMap
            VALUE value;
            return (TryGetValue(item.Key, out value) == true) && (ValueComparer.Equals(item.Value, value) == true);
        }

        public void CopyTo(KeyValuePair<KEY, VALUE>[] array, int arrayIndex)
        {
            ((IDictionary<KEY, VALUE>)KeyToValueMap).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<KEY, VALUE>> GetEnumerator()
        {
            return ((IDictionary<KEY, VALUE>)KeyToValueMap).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<KEY, VALUE>)KeyToValueMap).GetEnumerator();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return KeyToValueMap.ToString();
        }

        public override int GetHashCode()
        {
            return KeyToValueMap.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is BidirectionalDictionary<KEY, VALUE> other)
            {
                // Comparing the KeyToValueMap is really the only thing necessary for this
                return KeyToValueMap.Equals(other.KeyToValueMap);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
