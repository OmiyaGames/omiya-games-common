﻿using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <copyright file="OmiyaGamesUtility.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2018 Omiya Games
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
    /// A series of utilities used throughout the <code>OmiyaGames</code> namespace.
    /// </summary>
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    /// <listheader>
    /// <description>Date</description>
    /// <description>Name</description>
    /// <description>Description</description>
    /// </listheader>
    /// <item>
    /// <description>8/18/2015</description>
    /// <description>Taro</description>
    /// <description>Initial verison</description>
    /// 
    /// <description>6/4/2018</description>
    /// <description>Taro</description>
    /// <description>Added method for shortening URL</description>
    /// </item>
    /// </list>
    /// </remarks>
    public static class Utility
    {
        public const char PathDivider = '/';
        public const float SnapToThreshold = 0.01f;
        public const string FileExtensionScriptableObject = ".asset";
        public const string FileExtensionText = ".txt";
        public const string TimeStampPrint = "HH:mm:ss.ffff GMTzz";
        public const bool IsTimeStampPrintedByDefault = true;
        public static readonly string[] stripStartOfUrl = new string[]
        {
            "https://www.",
            "http://www.",
            "https://",
            "http://"
        };

        /// <summary>
        /// Creates a clone of the components <code>GameObject</code>, places it under
        /// the same parent on the hierarchy, and finally returns the copy of a component
        /// attached to that clone.
        /// </summary>
        /// <typeparam name="T">Component attached to a <code>GameObject</code></typeparam>
        /// <param name="copyFrom">The component to grab its <code>GameObject</code>.
        /// This will be used  to clone a new <code>GameObject</code>.</param>
        /// <param name="setActive">Whether the clone is active or not</param>
        /// <param name="copyPosition">Whether the clone will be at the same position
        /// as the original or not</param>
        /// <param name="copyRotation">Whether the clone will have the same rotation
        /// as the original or not</param>
        /// <param name="copyScale">Whether the clone will be scaled the same as the
        /// original or not</param>
        /// <returns>A component attached to the new clone</returns>
        public static T Replicate<T>(T copyFrom, bool setActive = true) where T : Component
        {
            // Create a clone
            GameObject clone = Replicate(copyFrom.gameObject, setActive);

            // Grab its component
            return clone.GetComponent<T>();
        }

        /// <summary>
        /// Creates a clone of the provided <code>GameObject</code> and places it under
        /// the assigned transform on the hierarchy.
        /// </summary>
        /// <typeparam name="T">Component attached to a <code>GameObject</code></typeparam>
        /// <param name="copyFrom">The component to grab its <code>GameObject</code>.
        /// <param name="attachTo">The <code>Transform</code> to make the clone a child of.
        /// <code>null</code> will place the clone at the hierarchy's root.</param>
        /// <param name="setActive">Whether the clone is active or not</param>
        /// <param name="copyLocalPosition">Whether the clone will be at the same position
        /// as the original or not</param>
        /// <param name="copyLocalRotation">Whether the clone will have the same rotation
        /// as the original or not</param>
        /// <param name="copyLocalScale">Whether the clone will be scaled the same as the
        /// original or not</param>
        /// <returns>A component attached to the new clone</returns>
        public static T Replicate<T>(T copyFrom, Transform attachTo, bool setActive = true, bool copyLocalPosition = true, bool copyLocalRotation = true, bool copyLocalScale = true) where T : Component
        {
            // Create a clone
            GameObject clone = Replicate(copyFrom.gameObject, copyFrom.transform.parent, setActive, copyLocalPosition, copyLocalRotation, copyLocalScale);

            // Grab its component
            return clone.GetComponent<T>();
        }

        /// <summary>
        /// Creates a clone of the provided <code>GameObject</code> and places it under
        /// the same parent on the hierarchy.
        /// </summary>
        /// <param name="copyFrom">The <code>GameObject</code> to clone off of.</param>
        /// <param name="setActive">Whether the clone is active or not</param>
        /// <returns>A clone of <code>GameObject</code></returns>
        public static GameObject Replicate(GameObject copyFrom, bool setActive = true)
        {
            return Replicate(copyFrom, copyFrom.transform.parent, setActive, true, true, true);
        }

        /// <summary>
        /// Creates a clone of the provided <code>GameObject</code> and places it under
        /// the assigned transform on the hierarchy.
        /// </summary>
        /// <param name="copyFrom">The <code>GameObject</code> to clone off of.</param>
        /// <param name="attachTo">The <code>Transform</code> to make the clone a child of.
        /// <code>null</code> will place the clone at the hierarchy's root.</param>
        /// <param name="setActive">Whether the clone is active or not</param>
        /// <param name="copyLocalPosition">Whether the clone will be at the same position
        /// as the original or not</param>
        /// <param name="copyLocalRotation">Whether the clone will have the same rotation
        /// as the original or not</param>
        /// <param name="copyLocalScale">Whether the clone will be scaled the same as the
        /// original or not</param>
        /// <returns>A clone of <code>GameObject</code></returns>
        public static GameObject Replicate(GameObject copyFrom, Transform attachTo, bool setActive = true, bool copyLocalPosition = true, bool copyLocalRotation = true, bool copyLocalScale = true)
        {
            // Create a clone
            GameObject clone = MonoBehaviour.Instantiate<GameObject>(copyFrom);

            // Setup its transform
            clone.transform.SetParent(attachTo, true);
            clone.transform.SetAsLastSibling();

            // Setup it's dimensions
            clone.SetActive(setActive);
            if (copyLocalPosition == true)
            {
                clone.transform.localPosition = copyFrom.transform.localPosition;
            }
            if (copyLocalRotation == true)
            {
                clone.transform.localRotation = copyFrom.transform.localRotation;
            }
            if (copyLocalScale == true)
            {
                clone.transform.localScale = copyFrom.transform.localScale;
            }
            return clone;
        }

        /// <summary>
        /// Shuffles the list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="upTo">Number of elements to shuffle, starting at index 0.
        /// Elements outside of this range maybe be shuffled between this range as well.
        /// If negative, will shuffle all list elements.</param>
        /// <typeparam name="H">The list type parameter.</typeparam>
        public static void ShuffleList<H>(IList<H> list, int upTo = -1)
        {
            // Check if we want to shuffle the entire list
            if ((upTo < 0) || (upTo > list.Count))
            {
                upTo = list.Count;
            }

            // Go through every list element
            H swapObject = default(H);
            int index = 0, randomIndex = 0;
            for (; index < upTo; ++index)
            {
                // Swap a random element
                randomIndex = Random.Range(0, list.Count);
                if (index != randomIndex)
                {
                    swapObject = list[index];
                    list[index] = list[randomIndex];
                    list[randomIndex] = swapObject;
                }
            }
        }

        public static void RemoveDuplicateEntries<H>(List<H> list, IEqualityComparer<H> comparer = null)
        {
            // Go through every list element
            int focusIndex = 0, compareIndex = 0;
            bool isDuplicate = false;
            for (; focusIndex < list.Count; ++focusIndex)
            {
                // Start the loop with the next element the next element
                for (compareIndex = (focusIndex + 1); compareIndex < list.Count; ++compareIndex)
                {
                    // Check if the elements are the same
                    if (comparer == null)
                    {
                        isDuplicate = list[focusIndex].Equals(list[compareIndex]);
                    }
                    else
                    {
                        isDuplicate = comparer.Equals(list[focusIndex], list[compareIndex]);
                    }

                    // Check if this element is a dupicate
                    if (isDuplicate == true)
                    {
                        // If so, remove from the list
                        list.RemoveAt(compareIndex);
                        --compareIndex;
                    }
                }
            }
        }

        public static void Log(string message, bool showTimestamp = IsTimeStampPrintedByDefault)
        {
#if DEBUG
            // Only do something if we're in debug mode
            if (showTimestamp == true)
            {
                message = '<' + System.DateTime.Now.ToString(TimeStampPrint) + "> " + message;
            }
            Debug.Log(message);
#endif
        }

        public static string ShortenUrl(string url)
        {
            foreach (string stripFromStart in stripStartOfUrl)
            {
                if (url.StartsWith(stripFromStart) == true)
                {
                    url = url.Remove(0, stripFromStart.Length);
                    break;
                }
            }
            url = url.TrimEnd('/');
            return url;
        }

        public static Canvas GetParentCanvas(Transform checkTransform)
        {
            // Check if it has a canvas
            Canvas parentCanvas = checkTransform.GetComponent<Canvas>();

            // Loop while canvas isn't set, and there is a parent to be concerned of
            while ((checkTransform != null) && (checkTransform.parent != null) && (parentCanvas == null))
            {
                // Grab the next parent
                checkTransform = checkTransform.parent;

                // Check if parent has a canvas
                parentCanvas = checkTransform.GetComponent<Canvas>();
            }
            return parentCanvas;
        }

        public static ENUM ConvertToEnum<ENUM>(int value) where ENUM : System.Enum
        {
            return (ENUM)System.Enum.ToObject(typeof(ENUM), value);
        }

        public static int ConvertToInt<ENUM>(ENUM value) where ENUM : System.Enum
        {
            return System.Convert.ToInt32(value);
        }

        /// <summary>
        /// A slightly more efficient way of setting a Vector3 than assignment.
        /// </summary>
        public static void SetVector(ref Vector3 toSet, ref Vector3 toCopy)
        {
            toSet.x = toCopy.x;
            toSet.y = toCopy.y;
            toSet.z = toCopy.z;
        }

        /// <summary>
        /// A slightly more efficient way of setting a Vector3 than assignment.
        /// </summary>
        public static void SetVector(ref Vector2 toSet, ref Vector2 copy)
        {
            toSet.x = copy.x;
            toSet.y = copy.y;
        }

        /// <summary>
        /// A slightly more efficient way of incrementing a Vector3 than assignment.
        /// </summary>
        public static void IncrementVector(ref Vector3 toSet, ref Vector3 add)
        {
            toSet.x += add.x;
            toSet.y += add.y;
            toSet.z += add.z;
        }

        /// <summary>
        /// A slightly more efficient way of incrementing a Vector3 than assignment.
        /// </summary>
        public static void IncrementVector(ref Vector2 toSet, ref Vector2 add)
        {
            toSet.x = add.x;
            toSet.y = add.y;
        }

        /// <summary>
        /// A slightly more efficient way of decrementing a Vector3 than assignment.
        /// </summary>
        public static void DecrementVector(ref Vector3 toSet, ref Vector3 subtract)
        {
            toSet.x -= subtract.x;
            toSet.y -= subtract.y;
            toSet.z -= subtract.z;
        }

        /// <summary>
        /// A slightly more efficient way of decrementing a Vector3 than assignment.
        /// </summary>
        public static void DecrementVector(ref Vector2 toSet, ref Vector2 subtract)
        {
            toSet.x -= subtract.x;
            toSet.y -= subtract.y;
        }

        /// <summary>
        /// Grabs a component, and sets it to cache, unless the cache isn't null.
        /// </summary>
        public static T GetComponentCached<T>(MonoBehaviour script, ref T cache) where T : Component
        {
            if(cache == null)
            {
                cache = script.GetComponent<T>();
            }
            return cache;
        }
    }
}
