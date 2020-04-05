using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

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
    /// A list that shuffles its elements. A common example:
    /// <code>
    /// int[] allNumbers = new int[] { 1, 2, 3, 4 };
    /// RandomList<int> shuffledNumbers = new RandomList<int>(allNumbers);
    /// for(int i = 0; i < allNumbers.Length; ++i)
    /// {
    ///     Debug.Log(shuffledNumbers.NextRandomElement);
    /// }
    /// </code>
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
    ///     <description>Initial version</description>
    ///   </item>
    ///   <item>
    ///     <description>3/25/2020</description>
    ///     <description>Taro</description>
    ///     <description>Converted the class to a package</description>
    ///   </item>
    ///   <item>
    ///     <description>4/5/2020</description>
    ///     <description>Taro</description>
    ///     <description>Updating to be serializable...albeit, in Unity 2020.1</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [System.Serializable]
    public class RandomList<T> : ICollection<T>, IEnumerable<T>, ICollection<RandomList<T>.ElementFrequency>, IEnumerable<RandomList<T>.ElementFrequency>
    {
        /// <summary>
        /// Indicates the frequency an element is going to be added into the index list,
        /// The higher the frequency, the more often an element appears.
        /// </summary>
        [System.Serializable]
        public struct ElementFrequency
        {
            [SerializeField]
            [Tooltip("An element in the RandomList.")]
            T element;
            [SerializeField]
            [Tooltip("Number of times the element is shuffled into the RandomList.")]
            int frequency;

            /// <summary>
            /// The element this struct is representing.
            /// </summary>
            public T Element
            {
                get => element;
                set => element = value;
            }

            /// <summary>
            /// The number of times this element appears in the shuffled index list.
            /// This value is never below 1
            /// </summary>
            public int Frequency
            {
                get
                {
                    // If somehow the frequency is below 1, force frequency to 1.
                    // Basically safe-guareding from Unity's serializion.
                    if (frequency < 1)
                    {
                        frequency = 1;
                    }
                    return frequency;
                }
                set
                {
                    // Prevent ferquency from going below zero
                    frequency = value;
                    if (frequency < 1)
                    {
                        frequency = 1;
                    }
                }
            }

            public ElementFrequency(T element, int frequency = 1)
            {
                this.element = element;
                this.frequency = frequency;

                // Force frequency to be floored to 1
                if (this.frequency < 1)
                {
                    this.frequency = 1;
                }
            }

            public override int GetHashCode()
            {
                return Element.GetHashCode() ^ Frequency.GetHashCode();
            }

            /// <summary>
            /// Checks the type of argument.
            /// If it's another <see cref="ElementFrequency"/>, compares both
            /// <see cref="Element"/> and <see cref="Frequency"/>.
            /// If it's <typeparamref name="T"/>, compares if
            /// <see cref="Element"/> matches with the argument.
            /// </summary>
            /// <param name="obj">The object to compare to.</param>
            /// <returns>
            /// If it's another <see cref="ElementFrequency"/>, returns true
            /// if both <see cref="Element"/> and <see cref="Frequency"/> matches.
            /// If it's <typeparamref name="T"/>, returns true
            /// if <see cref="Element"/> matches with the argument.
            /// Otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (obj is ElementFrequency other)
                {
                    return (other.Frequency == this.Frequency) && (Comparer<T>.Default.Compare(other.Element, this.Element) == 0);
                }
                else if (obj is T otherElement)
                {
                    return (Comparer<T>.Default.Compare(otherElement, this.Element) == 0);
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The serialized list of elements.
        /// </summary>
        [SerializeField]
        List<ElementFrequency> elementsList;
        /// <summary>
        /// Dictionary mapping an element to the index in <see cref="elementsList"/>.
        /// </summary>
        readonly Dictionary<T, int> elementToIndexMap;
        /// <summary>
        /// Contains a list of whole numbers corresponding to an index
        /// in <see cref="elementsList"/>. Note that the <see cref="ElementFrequency.Frequency"/>
        /// will affect the number of times an index is duplicated in this list.
        /// </summary>
        readonly List<int> randomizedIndexes;
        /// <summary>
        /// An index within <see cref="randomizedIndexes"/>.
        /// If it's *not* within, <see cref="randomizedIndexes"/>,
        /// the next time <see cref="CurrentElement"/> is called,
        /// it'll shuffle <see cref="randomizedIndexes"/>.
        /// </summary>
        int index = int.MinValue;

        /// <summary>
        /// Creates an empty list.
        /// </summary>
        public RandomList()
        {
            // Setup member variables
            elementsList = new List<ElementFrequency>();
            randomizedIndexes = new List<int>();
        }

        /// <summary>
        /// Copies the elements of the list,
        /// each with equal frequency of appearance,
        /// into a new <see cref="RandomList{T}"/>.
        /// </summary>
        public RandomList(IList<T> list)
        {
            // Check the validity of the argument
            if ((list != null) && (list.Count > 0))
            {
                // Setup member variables
                elementsList = new List<ElementFrequency>(list.Count);
                randomizedIndexes = new List<int>(list.Count);

                // Populate list
                for (int index = 0; index < list.Count; ++index)
                {
                    Add(list[index]);
                }
                SetupIndexList();
            }
            else
            {
                elementsList = new List<ElementFrequency>();
                randomizedIndexes = new List<int>();
            }
        }

        /// <summary>
        /// Copies the elements of a list into
        /// a new <see cref="RandomList{T}"/>.
        /// </summary>
        /// <param name="list"></param>
        public RandomList(IList<ElementFrequency> list)
        {
            // Check the validity of the argument
            if ((list != null) && (list.Count > 0))
            {
                // Setup member variables
                elementsList = new List<ElementFrequency>(list.Count);
                randomizedIndexes = new List<int>(list.Count);

                // Populate list
                for (int index = 0; index < list.Count; ++index)
                {
                    Add(list[index]);
                }
                SetupIndexList();
            }
            else
            {
                elementsList = new List<ElementFrequency>();
                randomizedIndexes = new List<int>();
            }
        }

        /// <summary>
        /// Number of elements in this list.
        /// Disregards the <see cref="ElementFrequency.Frequency"/> value.
        /// </summary>
        public int Count
        {
            get
            {
                return elementsList.Count;
            }
        }

        /// <summary>
        /// Grabs the currently focused element in the list.
        /// </summary>
        /// <remarks>
        /// This method shuffles <see cref="randomizedIndexes"/>
        /// if <see cref="index"/> is outside of the list's range.
        /// </remarks>
        /// <seealso cref="NextRandomElement"/>
        public T CurrentElement
        {
            get
            {
                T returnElement = default(T);
                if (Count == 1)
                {
                    // Grab the only element
                    if (elementsList != null)
                    {
                        returnElement = elementsList[0].Element;
                    }
                }
                else if (Count > 1)
                {
                    // Check if I need to setup a list
                    if (randomizedIndexes.Count <= 0)
                    {
                        SetupIndexList();
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
                    if (elementsList != null)
                    {
                        returnElement = elementsList[randomizedIndexes[index]].Element;
                    }
                }
                return returnElement;
            }
        }

        /// <summary>
        /// Grabs the next random element from the list.
        /// </summary>
        /// <seealso cref="CurrentElement"/>
        public T NextRandomElement
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

        /// <summary>
        /// Flags the list to reshuffle next time when
        /// <see cref="CurrentElement"/> or
        /// <see cref="NextRandomElement"/> gets called.
        /// </summary>
        public void Reshuffle()
        {
            index = int.MinValue;
        }

        /// <summary>
        /// Appends an item to the end of the list, paired with a frequency value.
        /// This method does *not* shuffle the list, thus making the item appear at the end of enumeration consistently.
        /// Remember to run <see cref="Reshuffle()"> after this method.
        /// </summary>
        public void Add(T item, int frequency)
        {
            Add(new ElementFrequency(item, frequency));
        }

        #region Interface Implementation
        /// <summary>
        /// Always returns true.
        /// </summary>
        /// <returns>true</returns>
        public bool IsReadOnly => false;

        /// <summary>
        /// Enumerates through all items, in order of appended elements.
        /// </summary>
        IEnumerator<ElementFrequency> IEnumerable<ElementFrequency>.GetEnumerator()
        {
            return elementsList.GetEnumerator();
        }

        /// <summary>
        /// Enumerates through all items, in order of appended elements.
        /// </summary>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach (ElementFrequency item in elementsList)
            {
                yield return item.Element;
            }
        }

        /// <summary>
        /// Enumerates through all items, in order of appended elements.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (ElementFrequency item in elementsList)
            {
                yield return item.Element;
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
        public void Add(ElementFrequency item)
        {
            elementsList.Add(item);
            if (randomizedIndexes.Count > 0)
            {
                int newIndex = elementsList.Count - 1;
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
            elementsList.Clear();
            randomizedIndexes.Clear();
            Reshuffle();
        }

        public bool Contains(T item)
        {
            return (IndexOf(item) >= 0);
        }

        public bool Contains(ElementFrequency item)
        {
            return (IndexOf(item) >= 0);
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
            else if (array.Length < (elementsList.Count + arrayIndex))
            {
                throw new System.ArgumentException("array is too small to copy to");
            }

            for (int offsetIndex = 0; offsetIndex < elementsList.Count; ++offsetIndex)
            {
                array[arrayIndex + offsetIndex] = elementsList[offsetIndex].Element;
            }
        }

        public void CopyTo(ElementFrequency[] array, int arrayIndex)
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
            else if (array.Length < (elementsList.Count + arrayIndex))
            {
                throw new System.ArgumentException("array is too small to copy to");
            }

            for (int offsetIndex = 0; offsetIndex < elementsList.Count; ++offsetIndex)
            {
                array[arrayIndex + offsetIndex] = elementsList[offsetIndex];
            }
        }

        /// <summary>
        /// Removes the first instance of the item from the list.
        /// This method does *not* shuffle the list: remember to run
        /// <see cref="Reshuffle()"> after this method.
        /// </summary>
        public bool Remove(T item)
        {
            // Actually check where the first instance of item is in the original list
            int removeIndex = IndexOf(item);

            // Check if this item is found
            bool returnFlag = false;
            if (removeIndex >= 0)
            {
                // If so, remove the element
                elementsList.RemoveAt(removeIndex);
                RemoveAllFromIndexList(removeIndex);
                returnFlag = true;
            }
            return returnFlag;
        }

        /// <summary>
        /// Removes the first instance of the item from the list.
        /// This method does *not* shuffle the list: remember to run
        /// <see cref="Reshuffle()"> after this method.
        /// </summary>
        public bool Remove(ElementFrequency item)
        {
            // Actually check where the first instance of item is in the original list
            int removeIndex = IndexOf(item);

            // Check if this item is found
            bool returnFlag = false;
            if (removeIndex >= 0)
            {
                // If so, remove the element
                elementsList.RemoveAt(removeIndex);
                RemoveAllFromIndexList(removeIndex);
                returnFlag = true;
            }
            return returnFlag;
        }
        #endregion

        #region Helper Methods
        /// <summary>
        /// Gets index of the first instance of item from <see cref="elementsList"/>.
        /// </summary>
        /// <param name="item">Element in <see cref="elementsList"/> to search for.</param>
        /// <returns>Index of item in  <see cref="elementsList"/>, or -1 if not found.</returns>
        int IndexOf(T item)
        {
            for (int removeIndex = 0; removeIndex < elementsList.Count; ++removeIndex)
            {
                if (elementsList[removeIndex].Equals(item) == true)
                {
                    return removeIndex;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets index of the first instance of item from <see cref="elementsList"/>.
        /// </summary>
        /// <param name="item">Item in <see cref="elementsList"/> to search for.</param>
        /// <returns>Index of item in  <see cref="elementsList"/>, or -1 if not found.</returns>
        int IndexOf(ElementFrequency item)
        {
            for (int removeIndex = 0; removeIndex < elementsList.Count; ++removeIndex)
            {
                if (elementsList[removeIndex].Equals(item) == true)
                {
                    return removeIndex;
                }
            }
            return -1;
        }

        /// <summary>
        /// Clears the index list, and repopulates it with corresponding indexes
        /// to <see cref="elementsList"/>. Note this method does duplicate indexes,
        /// based on <see cref="ElementFrequency.Frequency"/>.
        /// </summary>
        /// 
        void SetupIndexList()
        {
            // Generate a new list, populated with entries based on frequency
            randomizedIndexes.Clear();
            for (index = 0; index < Count; ++index)
            {
                for (int numAdded = 0; numAdded < elementsList[index].Frequency; ++numAdded)
                {
                    randomizedIndexes.Add(index);
                }
            }

            // Flag the list for re-shuffling
            Reshuffle();
        }

        /// <summary>
        /// Removes all instances of a value from <see cref="randomizedIndexes"/>,
        /// and decrements any other value greater than removeIndex.
        /// </summary>
        /// <param name="removeIndex">The value to remove from  <see cref="randomizedIndexes"/>.</param>
        void RemoveAllFromIndexList(int removeIndex)
        {
            // Shift every index in the indexes list
            int check = 0;
            while (check < randomizedIndexes.Count)
            {
                // Compare indexes
                // Note: doing less-than and greater-than comparisons first,
                // as they're more likely to occur, and it's (slightly) more
                // efficient to hit the earlier conditionals first.
                if (randomizedIndexes[check] < removeIndex)
                {
                    // If less, skip to the next index
                    ++check;
                }
                else if (randomizedIndexes[check] > removeIndex)
                {
                    // If greater, shift this index down one
                    randomizedIndexes[check] -= 1;

                    // Skip to the next index
                    ++check;
                }
                else
                {
                    // Remove this index
                    randomizedIndexes.RemoveAt(check);

                    // Don't change check; the line above will shift
                    // all elements by one, so we don't want to miss
                    // the next element.
                }
            }
        }
        #endregion
    }
}
