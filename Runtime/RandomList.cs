using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <copyright file="RandomList.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2020 Omiya Games
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
    /// <date>8/18/2015</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A list that shuffles its elements.
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
    ///     <description>8/18/2015</description>
    ///     <description>Taro</description>
    ///     <description>Initial verison</description>
    ///   </item>
    ///   <item>
    ///     <description>3/25/2020</description>
    ///     <description>Taro</description>
    ///     <description>Converted the class to a package</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public class RandomList<T> : ICollection<T>, IEnumerable<T>
    {
        /// <summary>
        /// Indicates the frequency an element is going to be added into the index list,
        /// The higher the frequency, the more often an element appears.
        /// </summary>
        [System.Serializable]
        public struct ElementFrequency
        {
            [SerializeField]
            T value;
            [SerializeField]
            int frequency;

            public T Value
            {
                get => value;
                set => this.value = value;
            }

            public int Frequency
            {
                get => frequency;
                set
                {
                    // Prevent ferquency from going below zero
                    frequency = value;
                    if(frequency < 1)
                    {
                        frequency = 1;
                    }
                }
            }

            public ElementFrequency(T value, int frequency = 1)
            {
                this.value = value;
                this.frequency = frequency;
                if(frequency < 1)
                {
                    frequency = 1;
                }
            }
        }

        readonly List<ElementFrequency> originalList;
        readonly List<int> randomizedIndexes;
        int index = int.MinValue;

        public RandomList()
        {
            // Setup member variables
            originalList = new List<ElementFrequency>();
            randomizedIndexes = new List<int>();
        }

        public RandomList(IList<T> list)
        {
            if(list == null)
            {
                throw new System.ArgumentNullException("list");
            }

            // Cache list size
            if(list.Count > 0)
            {
                // Setup member variables
                originalList = new List<ElementFrequency>(list.Count);
                randomizedIndexes = new List<int>(list.Count);

                // Populate list
                for(int index = 0; index < list.Count; ++index)
                {
                    originalList.Add(new ElementFrequency(list[index]));
                }
            }
            else
            {
                originalList = new List<ElementFrequency>();
                randomizedIndexes = new List<int>();
            }
        }

        public RandomList(IList<ElementFrequency> list)
        {
            if(list == null)
            {
                throw new System.ArgumentNullException("list");
            }

            // Cache list size
            if(list.Count > 0)
            {
                // Setup member variables
                originalList = new List<ElementFrequency>(list.Count);
                randomizedIndexes = new List<int>(list.Count);

                // Populate list
                for(int index = 0; index < list.Count; ++index)
                {
                    originalList.Add(list[index]);
                }
            }
            else
            {
                originalList = new List<ElementFrequency>();
                randomizedIndexes = new List<int>();
            }
        }

        public int Count
        {
            get
            {
                return originalList.Count;
            }
        }

        public T CurrentElement
        {
            get
            {
                T returnElement = default(T);
                if(Count == 1)
                {
                    // Grab the only element
                    if (originalList != null)
                    {
                        returnElement = originalList[0].Value;
                    }
                }
                else if (Count > 1)
                {
                    // Check if I need to setup a list
                    if (randomizedIndexes.Count <= 0)
                    {
                        SetupList();
                        Helpers.ShuffleList<int>(randomizedIndexes);
                        index = 0;
                    }
                    else if ((index >= randomizedIndexes.Count) || (index < 0))
                    {
                        // Shuffle the list if we got to the last element
                        Helpers.ShuffleList<int>(randomizedIndexes);
                        index = 0;
                    }

                    // Grab the current element
                    if (originalList != null)
                    {
                        returnElement = originalList[randomizedIndexes[index]].Value;
                    }
                }
                return returnElement;
            }
        }

        public T RandomElement
        {
            get
            {
                if (Count > 1)
                {
                    ++index;
                }
                return CurrentElement;
            }
        }

        public void Reshuffle()
        {
            index = int.MinValue;
        }

        #region Interface Implementation
        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <returns>true</returns>
        public bool IsReadOnly => true;

        /// <summary>
        /// Enumerates through all items. This order is neither affected by frequency or shuffling.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            foreach (ElementFrequency item in originalList)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Enumerates through all items. This order is neither affected by frequency or shuffling.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (ElementFrequency item in originalList)
            {
                yield return item.Value;
            }
        }

        /// <summary>
        /// Appends an item to the end of the list.
        /// This method does *not* shuffle the list, thus making the item appear at the end of enumeration consistently.
        /// Remember to run <see cref="Reshuffle()"> after this method.
        /// </summary>
        public void Add(T item)
        {
            Add(new ElementFrequency(item));
        }

        /// <summary>
        /// Appends an item to the end of the list.
        /// This method does *not* shuffle the list, thus making the item appear at the end of enumeration consistently.
        /// Remember to run <see cref="Reshuffle()"> after this method.
        /// </summary>
        public void Add(T item, int frequency)
        {
            Add(new ElementFrequency(item, frequency));
        }

        /// <summary>
        /// Appends an item to the end of the list.
        /// This method does *not* shuffle the list, thus making the item appear at the end of enumeration consistently.
        /// Remember to run <see cref="Reshuffle()"> after this method.
        /// </summary>
        public void Add(ElementFrequency item)
        {
            originalList.Add(item);
            if (randomizedIndexes.Count > 0)
            {
                int newIndex = originalList.Count - 1;
                for (int numAdded = 0; numAdded < item.Frequency; ++numAdded)
                {
                    randomizedIndexes.Add(newIndex);
                }
            }
        }

        /// <summary>
        /// Empties the list
        /// </summary>
        public void Clear()
        {
            originalList.Clear();
            randomizedIndexes.Clear();
        }

        public bool Contains(T item)
        {
            bool returnFlag = false;
            for (int checkIndex = 0; checkIndex < originalList.Count; ++checkIndex)
            {
                if (Comparer<T>.Default.Compare(item, originalList[checkIndex].Value) == 0)
                {
                    returnFlag = true;
                    break;
                }
            }
            return returnFlag;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
            {
                throw new System.ArgumentNullException("array");
            }
            else if (arrayIndex < 0)
            {
                throw new System.ArgumentOutOfRangeException("arrayIndex");
            }
            else if (array.Rank > 1)
            {
                throw new System.ArgumentException("array isn't one-dimensional");
            }
            else if (array.Length < (originalList.Count + arrayIndex))
            {
                throw new System.ArgumentException("array is too small to copy to");
            }

            for (int offsetIndex = 0; offsetIndex < originalList.Count; ++offsetIndex)
            {
                array[arrayIndex + offsetIndex] = originalList[offsetIndex].Value;
            }
        }

        public bool Remove(T item)
        {
            bool returnFlag = false;

            // Actually check where the first instance of item is in the original list
            int removeIndex = 0;
            for (; removeIndex < originalList.Count; ++removeIndex)
            {
                if (Comparer<T>.Default.Compare(item, originalList[removeIndex].Value) == 0)
                {
                    returnFlag = true;
                    break;
                }
            }

            // Check if this item is found
            if (returnFlag == true)
            {
                // If so, remove the element
                originalList.RemoveAt(removeIndex);

                // Shift every index in the indexes list
                int check = 0;
                while (check < randomizedIndexes.Count)
                {
                    // Compare indexes
                    if (randomizedIndexes[check] > removeIndex)
                    {
                        // Since an element is removed, shift the indexes down one
                        randomizedIndexes[check] -= 1;

                        // Skip to the next index
                        ++check;
                    }
                    else if (randomizedIndexes[check] < removeIndex)
                    {
                        // Skip to the next index
                        ++check;
                    }
                    else
                    {
                        // Remove this index; don't change check
                        randomizedIndexes.RemoveAt(check);
                    }
                }
            }
            return returnFlag;
        }
        #endregion

        #region Helper Methods

        void SetupList()
        {
            // Generate a new list, populated with entries based on frequency
            randomizedIndexes.Clear();
            for (index = 0; index < Count; ++index)
            {
                for (int numAdded = 0; numAdded < originalList[index].Frequency; ++numAdded)
                {
                    randomizedIndexes.Add(index);
                }
            }
        }
		#endregion
	}
}
