using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
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
        public void TestConstructorIListT()
        {
            // Setup loop variables
            RandomList<int> testList;
            List<int> referenceList = new List<int>(3);

            // Loop for unique elements
            for (int size = 1; size <= 3; ++size)
            {
                // Test the list constructors, and whether it copy its content correctly
                referenceList.Add(size);
                testList = new RandomList<int>(referenceList);
                
                // Run some tests
                TestRandomListMeta(testList, size, size, "Testing from TestConstructorIListT, unique elements.");
                TestRandomListContent(testList, 1, "Testing from TestConstructorIListT, unique elements.");
            }

            // Loop for same elements
            referenceList.Clear();
            for (int size = 1; size <= 3; ++size)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                referenceList.Add(1);
                testList = new RandomList<int>(referenceList);

                // Run some tests
                TestRandomListMeta(testList, 1, size, "Testing from TestConstructorIListT, same elements.");
                TestRandomListContent(testList, size, "Testing from TestConstructorIListT, same elements.");
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(IList{RandomList{T}.ElementFrequency})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(IList{RandomList{T}.ElementFrequency})"/>
        [Test]
        public void TestConstructorIListE()
        {
            // Setup loop variables
            RandomList<int> testList;
            RandomList<int>.ElementFrequency newFrequency;
            List<RandomList<int>.ElementFrequency> referenceList = new List<RandomList<int>.ElementFrequency>(3);
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(3);

            // Loop for unique elements
            for (int size = 1; size <= 3; ++size)
            {
                // Add new element to loop variables
                newFrequency = new RandomList<int>.ElementFrequency(size, size);
                referenceList.Add(newFrequency);
                referenceFrequencies.Add(newFrequency.Element, newFrequency.Frequency);

                // Test the list constructors, and whether it copy its content correctly
                testList = new RandomList<int>(referenceList);

                // Run some tests
                TestRandomListMeta(testList, size, size, "Testing from TestConstructorIListE, unique elements.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorIListE, unique elements.");
            }

            // Setup variables for next loop
            newFrequency = new RandomList<int>.ElementFrequency(1, 2);
            referenceList.Clear();
            referenceFrequencies.Clear();
            referenceFrequencies.Add(newFrequency.Element, 0);

            // Loop for same elements
            for (int size = 1; size <= 3; ++size)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                referenceList.Add(newFrequency);
                referenceFrequencies[newFrequency.Element] += newFrequency.Frequency;
                testList = new RandomList<int>(referenceList);

                // Run some tests
                TestRandomListMeta(testList, 1, size, "Testing from TestConstructorIListE, same elements.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorIListE, same elements.");
            }
        }


        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(IList{T}, IEqualityComparer{T})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(IList{T}, IEqualityComparer{T})"/>
        [Test]
        public void TestConstructorIListTIEqualityComparer()
        {
            // Setup loop variables
            RandomList<int> testList;
            List<int> referenceList = new List<int>(3);

            // Loop for unique elements
            for (int size = 1; size <= 3; ++size)
            {
                // Test the list constructors, and whether it copy its content correctly
                referenceList.Add(size);
                testList = new RandomList<int>(referenceList, testComparer);

                // Run some tests
                TestRandomListMeta(testList, size, size, testComparer, "Testing from TestConstructorIListTIEqualityComparer, unique elements.");
                TestRandomListContent(testList, 1, "Testing from TestConstructorIListTIEqualityComparer, unique elements.");
            }

            // Loop for same elements
            referenceList.Clear();
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>() {
                { 10, 0 }
            };
            for (int size = 1; size <= 3; ++size)
            {
                // Add a number
                referenceList.Add(10 * size);
                referenceFrequencies[10] += 1;

                // Test the capacity constructors, and whether it creates an empty dictionary
                testList = new RandomList<int>(referenceList, testComparer);

                // Run some tests
                TestRandomListMeta(testList, 1, size, testComparer, "Testing from TestConstructorIListTIEqualityComparer, same elements.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorIListTIEqualityComparer, same elements.");
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(IList{RandomList{T}.ElementFrequency}, IEqualityComparer{T})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(IList{RandomList{T}.ElementFrequency}, IEqualityComparer{T})"/>
        [Test]
        public void TestConstructorIListEIEqualityComparer()
        {
            // Setup loop variables
            RandomList<int> testList;
            RandomList<int>.ElementFrequency newFrequency;
            List<RandomList<int>.ElementFrequency> referenceList = new List<RandomList<int>.ElementFrequency>(3);
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(3);

            // Loop for unique elements
            for (int size = 1; size <= 3; ++size)
            {
                // Add new element to loop variables
                newFrequency = new RandomList<int>.ElementFrequency(size, size);
                referenceList.Add(newFrequency);
                referenceFrequencies.Add(newFrequency.Element, newFrequency.Frequency);

                // Test the list constructors, and whether it copy its content correctly
                testList = new RandomList<int>(referenceList, testComparer);

                // Run some tests
                TestRandomListMeta(testList, size, size, testComparer, "Testing from TestConstructorIListEIEqualityComparer, unique elements.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorIListEIEqualityComparer, unique elements.");
            }

            // Setup variables for next loop
            referenceList.Clear();
            referenceFrequencies.Clear();
            referenceFrequencies.Add(10, 0);

            // Loop for same elements
            for (int size = 1; size <= 3; ++size)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                newFrequency = new RandomList<int>.ElementFrequency(10 * size, size);
                referenceList.Add(newFrequency);
                referenceFrequencies[10] += newFrequency.Frequency;
                testList = new RandomList<int>(referenceList, testComparer);

                // Run some tests
                TestRandomListMeta(testList, 1, size, testComparer, "Testing from TestConstructorIListEIEqualityComparer, same elements.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorIListEIEqualityComparer, same elements.");
            }
        }
        #endregion

        #region Test Add
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T)"/>
        [Test]
        public void TestAddT()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Add(T)
            // FIXME: also test IEqualityComparer
            testList = new RandomList<int>(testComparer);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T, int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T, int)"/>
        [Test]
        public void TestAddTInt()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Add(T, int)
            // FIXME: also test IEqualityComparer
            testList = new RandomList<int>(testComparer);
        }
        #endregion

        #region Test Remove
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Remove(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Remove(T)"/>
        [Test]
        public void TestRemoveT()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Remove(T)
            // FIXME: also test IEqualityComparer
            // FIXME: don't forget to test edge cases, e.g. remove of elements that isn't in the list.
            testList = new RandomList<int>(testComparer);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Remove(T, int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Remove(T, int)"/>
        [Test]
        public void TestRemoveTInt()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Remove(T, int)
            // FIXME: also test IEqualityComparer
            // FIXME: don't forget to test edge cases, e.g
            // 1: remove of elements that isn't in the list.
            // 2: integer value that isn't in range, both directions.
            testList = new RandomList<int>(testComparer);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RemoveAllOf(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RemoveAllOf(T)"/>
        [Test]
        public void TestRemoveAllOfT()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test RemoveAllOf(T)
            // FIXME: also test IEqualityComparer
            // FIXME: don't forget to test edge cases, e.g. remove of elements that isn't in the list.
            testList = new RandomList<int>(testComparer);
        }
        #endregion

        #region Test Clear
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Clear()"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Clear()"/>
        [Test]
        public void TestClear()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Clear(T)
            // both when the list is empty, and has some elements in it.
        }
        #endregion

        #region Test Element Properties
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.CurrentElement"/> and <see cref="RandomList{T}.NextRandomElement"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.CurrentElement"/>
        /// <seealso cref="RandomList{T}.NextRandomElement"/>
        [Test]
        public void TestElementProperties()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test NextRandomElement and CurrentElement first under certain edge case circumstances, then make sure the former determines the value of the latter.
        }
        #endregion

        #region Test Reshuffle
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Reshuffle()"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Reshuffle()"/>
        [Test]
        public void TestReshuffle()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Reshuffle(T) by using Random.seed and Helper.ShuffleList
        }
        #endregion

        #region Test Contains
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Contains(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Contains(T)"/>
        [Test]
        public void TestContains()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test Contains(T) under edge cases
        }
        #endregion

        #region Test GetFrequency
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.GetFrequency(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.GetFrequency(T)"/>
        [Test]
        public void TestGetFrequency()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test GetFrequency(T) under edge cases
        }
        #endregion

        #region Test CopyTo
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.CopyTo(T[], int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.CopyTo(T[], int)"/>
        [Test]
        public void TestCopyToT()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test GetFrequency(T) under edge cases
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.CopyTo(RandomList{T}.ElementFrequency[], int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.CopyTo(RandomList{T}.ElementFrequency[], int)"/>
        [Test]
        public void TestCopyToE()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();
            // FIXME: test GetFrequency(T) under edge cases
        }
        #endregion

        #region Helper Methods
        private static void TestRandomListMeta(RandomList<int> testList, int expectedSize, int expectedCapacity, string message)
        {
            TestRandomListMeta(testList, expectedSize, expectedCapacity, null, message);
        }

        private static void TestRandomListMeta(RandomList<int> testList, int expectedSize, int expectedCapacity, IEqualityComparer<int> expectedComparer, string message)
        {
            // Run tests
            Assert.IsNotNull(testList, message);
            Assert.AreEqual(expectedSize, testList.Count, message);
            Assert.AreEqual(expectedCapacity, testList.Capacity, message);
            if(expectedComparer != null)
            {
                Assert.AreEqual(expectedComparer, testList.Comparer, message);
            }
        }

        private static void TestRandomListContent(RandomList<int> testList, int expectedFrequency, string message)
        {
            // Make sure the list is initialized correctly
            int index = 1;
            foreach (int element in testList)
            {
                Assert.AreEqual(index, element, message);
                Assert.AreEqual(expectedFrequency, testList.GetFrequency(element), message);
                ++index;
            }
        }

        private static void TestRandomListContent<T>(RandomList<T> testList, IDictionary<T, int> expectedFrequencies, string message)
        {
            // Make sure the list is initialized correctly
            int count = 0;
            foreach (T element in testList)
            {
                Assert.IsTrue(expectedFrequencies.ContainsKey(element), message);
                Assert.AreEqual(expectedFrequencies[element], testList.GetFrequency(element), message);
                ++count;
            }

            // Make sure all dictionary elements were matched
            Assert.AreEqual(expectedFrequencies.Count, count, "Did all frequency match?");
        }
        #endregion
    }
}
