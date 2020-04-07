using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OmiyaGames.Common.Runtime.Tests
{
    ///-----------------------------------------------------------------------
    /// <copyright file="TestRandomList.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020 Omiya Games
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
    /// <date>4/6/2020</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Unit test script for <see cref="RandomList{T}"/>.
    /// </summary>
    /// 
    /// <seealso cref="RandomList{T}"/>
    /// 
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    /// <listheader>
    ///   <description>Date</description>
    ///   <description>Author</description>
    ///   <description>Description</description>
    /// </listheader>
    /// <item>
    ///   <description>4/6/2020</description>
    ///   <description>Taro</description>
    ///   <description>Initial verison</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class TestRandomList
    {
        /// <summary>
        /// A <see cref="SingleDigitEqualityComparer"/> for testing purposes.
        /// </summary>
        readonly IEqualityComparer<int> testComparer = new SingleDigitEqualityComparer();

        #region Test Constructors
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList"/>
        [Test]
        public void TestConstructorDefault()
        {
            // Test the default constructor, and whether it creates an empty dictionary
            RandomList<string> testList = new RandomList<string>();

            // Run tests
            Assert.IsNotNull(testList);
            Assert.AreEqual(0, testList.Count);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(int)"/>
        [Test]
        public void TestConstructorInt()
        {
            RandomList<string> testList;
            for (int capacity = 10; capacity <= 30; capacity += 10)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                testList = new RandomList<string>(capacity);

                // Run tests
                Assert.IsNotNull(testList);
                Assert.AreEqual(0, testList.Count);
                Assert.AreEqual(capacity, testList.Capacity);
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(IEqualityComparer{T})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(IEqualityComparer{T})"/>
        [Test]
        public void TestConstructorIEqualityComparer()
        {
            // Test the capacity constructors, and whether it creates an empty dictionary
            RandomList<int> testList = new RandomList<int>(testComparer);

            // Run tests
            Assert.IsNotNull(testList);
            Assert.AreEqual(0, testList.Count);
            Assert.AreEqual(testComparer, testList.Comparer);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(int, IEqualityComparer{T})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(int, IEqualityComparer{T})"/>
        [Test]
        public void TestConstructorIntIEqualityComparer()
        {
            RandomList<int> testList;
            for (int capacity = 10; capacity <= 30; capacity += 10)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                testList = new RandomList<int>(capacity, testComparer);

                // Run tests
                Assert.IsNotNull(testList);
                Assert.AreEqual(0, testList.Count);
                Assert.AreEqual(capacity, testList.Capacity);
                Assert.AreEqual(testComparer, testList.Comparer);
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(IList{T})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(IList{T})"/>
        [Test]
        public void TestConstructorIList()
        {
            RandomList<int> testList;
            List<int> referenceList = new List<int>(3);
            for (int size = 1; size <= 3; ++size)
            {
                // Test the list constructors, and whether it copy its content correctly
                referenceList.Add(size);
                testList = new RandomList<int>(referenceList);

                // Run tests
                Assert.IsNotNull(testList);
                Assert.AreEqual(size, testList.Count, "Testing list size when each element is unique");
                Assert.AreEqual(size, testList.Count);

                // Make sure the list is initialized correctly
                int index = 1;
                foreach(int element in testList)
                {
                    Assert.AreEqual(index, element, "Testing list element when each element is unique");
                    Assert.AreEqual(1, testList.GetFrequency(element), "Testing list frequency when each element is unique");
                    ++index;
                }
            }

            referenceList.Clear();
            for (int size = 1; size <= 3; ++size)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                referenceList.Add(1);
                testList = new RandomList<int>(referenceList);

                // Run tests
                Assert.IsNotNull(testList);
                Assert.AreEqual(1, testList.Count, "Testing list size when each element is the same");
                Assert.AreEqual(size, testList.Capacity);

                // Make sure the list is initialized correctly
                foreach (int element in testList)
                {
                    Assert.AreEqual(1, element, "Testing list element when each element is the same");
                    Assert.AreEqual(size, testList.GetFrequency(element), "Testing list frequency when each element is the same");
                }
            }
        }
        #endregion
    }
}
