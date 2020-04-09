using System;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

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

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(Dictionary{T, int})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(Dictionary{T, int})"/>
        [Test]
        public void TestConstructorDictionaryTInt()
        {
            // Setup loop variables
            RandomList<int> testList;
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(3);

            // Loop for unique elements
            for (int size = 1; size <= 3; ++size)
            {
                // Add new element to loop variables
                referenceFrequencies.Add(size, size);

                // Test the list constructors, and whether it copy its content correctly
                testList = new RandomList<int>(referenceFrequencies);

                // Run some tests
                TestRandomListMeta(testList, size, size, referenceFrequencies.Comparer, "Testing from TestConstructorDictionaryT, default comparer.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorDictionaryT, default comparer.");
            }

            // Setup variables for next loop
            referenceFrequencies = new Dictionary<int, int>(3, testComparer);

            // Loop for same elements
            for (int size = 1; size <= 3; ++size)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                referenceFrequencies.Add(size, size);
                testList = new RandomList<int>(referenceFrequencies);

                // Run some tests
                TestRandomListMeta(testList, referenceFrequencies.Count, referenceFrequencies.Count, testComparer, "Testing from TestConstructorDictionaryT, test comparer.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorDictionaryT, test comparer.");
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.RandomList(IDictionary{T, int}, IEqualityComparer{T})"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.RandomList(IDictionary{T, int}, IEqualityComparer{T})"/>
        [Test]
        public void TestConstructorIDictionaryTIntComparer()
        {
            // Setup loop variables
            RandomList<int> testList;
            IDictionary<int, int> referenceFrequencies = new Dictionary<int, int>(3, testComparer);

            // Loop for same elements
            for (int size = 1; size <= 3; ++size)
            {
                // Test the capacity constructors, and whether it creates an empty dictionary
                referenceFrequencies.Add(size, size);
                testList = new RandomList<int>(referenceFrequencies, testComparer);

                // Run some tests
                TestRandomListMeta(testList, referenceFrequencies.Count, referenceFrequencies.Count, testComparer, "Testing from TestConstructorIDictionaryTIntComparer, test comparer.");
                TestRandomListContent(testList, referenceFrequencies, "Testing from TestConstructorIDictionaryTIntComparer, test comparer.");
            }
        }
        #endregion

        #region Test Add
        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T)"/>
        [Test]
        public void TestAddTNoConflicts()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>(3);
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(3);

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(size);
                referenceFrequencies.Add(size, 1);

                // Verify content of randomList
                Assert.AreEqual(size, testList.Count);
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTNoConflicts");
            }

            // Also test IEqualityComparer
            testList = new RandomList<int>(3, testComparer);
            referenceFrequencies.Clear();

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(size);
                referenceFrequencies.Add(size, 1);

                // Verify content of randomList
                Assert.AreEqual(size, testList.Count);
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTNoConflicts");
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T)"/>
        [Test]
        public void TestAddTWithConflicts()
        {
            // Start with an empty list
            const int toAdd = 10;
            RandomList<int> testList = new RandomList<int>(3);
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(1)
            {
                { toAdd, 0 }
            };

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(toAdd);
                referenceFrequencies[toAdd] += 1;

                // Verify content of randomList
                Assert.AreEqual(1, testList.Count);
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTWithConflicts");
            }

            // Also test IEqualityComparer
            testList = new RandomList<int>(3, testComparer);
            referenceFrequencies[toAdd] = 0;

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(toAdd * size);
                referenceFrequencies[toAdd] += 1;

                // Verify content of randomList
                Assert.AreEqual(1, testList.Count);
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTWithConflicts");
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T, int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T, int)"/>
        [Test]
        public void TestAddTIntExceptions()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();

            // Check if running Add with 0 as numberOfItemsToAdd throws an error
            ArgumentOutOfRangeException exception = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testList.Add(0, 0);
            }, "Testing TestAddTExceptions");
            Assert.NotNull(exception);
            Assert.IsNotEmpty(exception.Message);
            Assert.AreEqual("numberOfItemsToAdd", exception.ParamName);

            // Check if running Add with -1 as numberOfItemsToAdd throws an error
            exception = Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testList.Add(0, -1);
            }, "Testing TestAddTExceptions");
            Assert.NotNull(exception);
            Assert.IsNotEmpty(exception.Message);
            Assert.AreEqual("numberOfItemsToAdd", exception.ParamName);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T, int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T, int)"/>
        [Test]
        public void TestAddTIntNoConflicts()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>(3);
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(3);

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(size, size);
                referenceFrequencies.Add(size, size);

                // Verify content of randomList
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTNoConflicts");
            }

            // Also test IEqualityComparer
            testList = new RandomList<int>(3, testComparer);
            referenceFrequencies.Clear();

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(size, size);
                referenceFrequencies.Add(size, size);

                // Verify content of randomList
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTNoConflicts");
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Add(T, int)"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Add(T, int)"/>
        [Test]
        public void TestAddTIntWithConflicts()
        {
            // Start with an empty list
            const int toAdd = 10;
            RandomList<int> testList = new RandomList<int>(3);
            Dictionary<int, int> referenceFrequencies = new Dictionary<int, int>(1)
            {
                { toAdd, 0 }
            };

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add(toAdd, size);
                referenceFrequencies[toAdd] += size;

                // Verify content of randomList
                Assert.AreEqual(1, testList.Count);
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTIntWithConflicts");
            }

            // Also test IEqualityComparer
            testList = new RandomList<int>(3, testComparer);
            referenceFrequencies[toAdd] = 0;

            // Test Add(T)
            for (int size = 1; size <= 3; ++size)
            {
                // Perform action
                testList.Add((toAdd * size), size);
                referenceFrequencies[toAdd] += size;

                // Verify content of randomList
                Assert.AreEqual(1, testList.Count);
                TestRandomListContent(testList, referenceFrequencies, "Testing TestAddTIntWithConflicts");
            }
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

            // test Clear() when list is empty from the first place
            testList.Clear();
            Assert.AreEqual(0, testList.Count);
            foreach (int i in testList)
            {
                Assert.Fail("Should not be able to enumerate an empty list.");
                break;
            }

            // Add a bunch of elements into the list
            const int listSize = 3;
            for (int size = 1; size <= listSize; ++size)
            {
                testList.Add(size, size);
            }
            Assert.AreEqual(listSize, testList.Count);

            // test Clear() when list has some elements
            testList.Clear();
            Assert.AreEqual(0, testList.Count);
            foreach (int i in testList)
            {
                Assert.Fail("Should not be able to enumerate an empty list.");
                break;
            }
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
            // Create a list to test sorting on
            List<int> testBase = new List<int>();
            for (int numAdd = 1; numAdd <= 3; ++numAdd)
            {
                for (int instances = 0; instances < numAdd; ++instances)
                {
                    testBase.Add(numAdd);
                }
            }

            // Start with a filled list
            RandomList<int> testList = new RandomList<int>(testBase);

            // RandomList uses ShuffleList, so use that as basis to confirm the shuffling worked
            Random.State lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Verify a small edge case where CurrentElement gets called first
            Random.state = lastState;
            Assert.AreEqual(testBase[0], testList.CurrentElement);

            // Go call the properties to confirm the rest of their values
            for (int i = 1; i < testBase.Count; ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);

                // Also verify CurrentElement is the same value
                Assert.AreEqual(testElement, testList.CurrentElement);
            }

            // Double-check that the next go-around, the list is re-shuffled again.
            lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            for (int i = 0; i < testBase.Count; ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);

                // Also verify CurrentElement is the same value
                Assert.AreEqual(testElement, testList.CurrentElement);
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.CurrentElement"/> and <see cref="RandomList{T}.NextRandomElement"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.CurrentElement"/>
        /// <seealso cref="RandomList{T}.NextRandomElement"/>
        [Test]
        public void TestElementPropertiesEdgeCases()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>(4);

            // Confirm Properties return default values
            Assert.AreEqual(default(int), testList.CurrentElement);
            Assert.AreEqual(default(int), testList.NextRandomElement);
            Assert.AreEqual(default(int), testList.CurrentElement);

            // Add only a single value
            const int testValue = 10;
            testList.Add(testValue);

            // Confirm Properties return only this value
            Assert.AreEqual(testValue, testList.CurrentElement);
            Assert.AreEqual(testValue, testList.NextRandomElement);
            Assert.AreEqual(testValue, testList.CurrentElement);

            // Add the test value a few more times
            for (int i = 0; i < 3; ++i)
            {
                testList.Add(testValue);
            }

            // Confirm Properties return only this value
            for (int i = 0; i < testList.GetFrequency(testValue); ++i)
            {
                Assert.AreEqual(testValue, testList.CurrentElement);
                Assert.AreEqual(testValue, testList.NextRandomElement);
                Assert.AreEqual(testValue, testList.CurrentElement);
            }

            // Add element, shuffle list, then remove the same element
            testList.Add(testValue + 1);
            int testElement = testList.NextRandomElement;
            testList.Remove(testValue + 1);

            // Confirm Properties return only this value
            for (int i = 0; i < testList.GetFrequency(testValue); ++i)
            {
                Assert.AreEqual(testValue, testList.CurrentElement);
                Assert.AreEqual(testValue, testList.NextRandomElement);
                Assert.AreEqual(testValue, testList.CurrentElement);
            }

            // re-create the list, but with the test comparer
            testList = new RandomList<int>(3, testComparer);

            // Add the test value a few more times
            for (int i = 1; i <= 3; ++i)
            {
                testList.Add(testValue);
            }

            // Confirm Properties return only this value
            for (int i = 0; i < testList.GetFrequency(testValue); ++i)
            {
                Assert.AreEqual(testValue, testList.CurrentElement);
                Assert.AreEqual(testValue, testList.NextRandomElement);
                Assert.AreEqual(testValue, testList.CurrentElement);
            }

            // Add element, shuffle list, then remove the same element
            testList.Add(testValue + 1);
            testElement = testList.NextRandomElement;
            testList.Remove(testValue + 1);

            // Confirm Properties return only this value
            for (int i = 0; i < testList.GetFrequency(testValue); ++i)
            {
                Assert.AreEqual(testValue, testList.CurrentElement);
                Assert.AreEqual(testValue, testList.NextRandomElement);
                Assert.AreEqual(testValue, testList.CurrentElement);
            }
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
            // Create a list to test sorting on
            List<int> testBase = new List<int>();
            for (int numAdd = 1; numAdd <= 3; ++numAdd)
            {
                for (int instances = 0; instances < numAdd; ++instances)
                {
                    testBase.Add(numAdd);
                }
            }

            // Start with a filled list
            RandomList<int> testList = new RandomList<int>(testBase);

            // RandomList uses ShuffleList, so use that as basis to confirm the shuffling worked
            Random.State lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Verify a small edge case where CurrentElement gets called first
            Random.state = lastState;
            testList.Reshuffle();
            Assert.AreEqual(testBase[0], testList.CurrentElement);

            // Go call the properties to confirm the rest of their values
            for (int i = 1; i < testBase.Count; ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }

            // Double-check that the next go-around, the first half of the list is re-shuffled again.
            lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            testList.Reshuffle();
            for (int i = 0; i < (testBase.Count / 2); ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }

            // Now that the current element is somewhere in the middle of the RandomList,
            // really confirm Reshuffle works as expected
            lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            testList.Reshuffle();
            for (int i = 0; i < testBase.Count; ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Reshuffle()"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Reshuffle()"/>
        [Test]
        public void TestReshuffleEdgeCases()
        {
            // Start with an empty list
            RandomList<int> testList = new RandomList<int>();

            // Confirm Properties return default values, even if shuffled
            testList.Reshuffle();
            Assert.AreEqual(default(int), testList.CurrentElement);
            testList.Reshuffle();
            Assert.AreEqual(default(int), testList.NextRandomElement);

            // Add only a single value
            const int testValue = 10;
            testList.Add(testValue);

            // Confirm Properties return only this value, even if shuffled
            testList.Reshuffle();
            Assert.AreEqual(testValue, testList.CurrentElement);
            testList.Reshuffle();
            Assert.AreEqual(testValue, testList.NextRandomElement);
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Reshuffle()"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Reshuffle()"/>
        [Test]
        public void TestReshuffleAfterAdd()
        {
            // Create a list to test sorting on
            List<int> testBase = new List<int>();
            for (int numAdd = 1; numAdd <= 3; ++numAdd)
            {
                for (int instances = 0; instances < numAdd; ++instances)
                {
                    testBase.Add(numAdd);
                }
            }

            // Start with a filled list
            RandomList<int> testList = new RandomList<int>(testBase);

            // RandomList uses ShuffleList, so use that as basis to confirm the shuffling worked
            Random.State lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            testList.Reshuffle();
            for (int i = 0; i < (testBase.Count / 2); ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }

            // Append more elements to both lists
            const int newNumAdd = 4;
            for (int instances = 0; instances < newNumAdd; ++instances)
            {
                testBase.Add(newNumAdd);
                testList.Add(newNumAdd);
            }

            // Shuffle both lists, and compare results
            lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            testList.Reshuffle();
            for (int i = 0; i < (testBase.Count / 2); ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }
        }

        /// <summary>
        /// Unit test for <see cref="RandomList{T}.Reshuffle()"/>
        /// </summary>
        /// <seealso cref="RandomList{T}.Reshuffle()"/>
        [Test]
        public void TestReshuffleAfterRemove()
        {
            // Create a list to test sorting on
            const int numRemove = 4;
            List<int> testBase = new List<int>();
            for (int numAdd = 1; numAdd <= numRemove; ++numAdd)
            {
                for (int instances = 0; instances < numAdd; ++instances)
                {
                    testBase.Add(numAdd);
                }
            }

            // Start with a filled list
            RandomList<int> testList = new RandomList<int>(testBase);

            // RandomList uses ShuffleList, so use that as basis to confirm the shuffling worked
            Random.State lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            testList.Reshuffle();
            for (int i = 0; i < (testBase.Count / 2); ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }

            // Remove elements from both lists
            testList.RemoveAllOf(numRemove);
            while (testBase.Contains(numRemove) == true)
            {
                testBase.Remove(numRemove);
            }

            // Shuffle both lists, and compare results
            lastState = Random.state;
            Helpers.ShuffleList(testBase);

            // Go call the properties to confirm the rest of their values
            Random.state = lastState;
            testList.Reshuffle();
            for (int i = 0; i < (testBase.Count / 2); ++i)
            {
                // Test NextRandomElement (preventing it from getting called twice by accident)
                int testElement = testList.NextRandomElement;
                Assert.AreEqual(testBase[i], testElement);
            }
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
            const int fillTo = 3, testTo = fillTo + 3;
            RandomList<int> testList = new RandomList<int>();

            // test Contains where the list is empty
            for (int i = 1; i <= testTo; ++i)
            {
                Assert.IsFalse(testList.Contains(i));
            }

            // Fill in the list
            for (int addNum = 1; addNum <= fillTo; ++addNum)
            {
                testList.Add(addNum);

                // test Contains where the list has some elements
                for (int testNum = 1; testNum <= testTo; ++testNum)
                {
                    Assert.AreEqual((testNum <= addNum), testList.Contains(testNum));
                }
            }

            // Remove elements from the list
            for (int removeNum = fillTo; removeNum >= 1; --removeNum)
            {
                testList.Remove(removeNum);

                // test Contains where the list has some elements
                for (int testNum = 1; testNum <= testTo; ++testNum)
                {
                    Assert.AreEqual((testNum < removeNum), testList.Contains(testNum));
                }
            }
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
            const int fillTo = 3, testTo = fillTo + 3;
            RandomList<int> testList = new RandomList<int>();

            // test GetFrequency where the list is empty
            for (int i = 1; i <= testTo; ++i)
            {
                Assert.AreEqual(0, testList.GetFrequency(i));
            }

            // Fill in the list
            for (int addNum = 1; addNum <= fillTo; ++addNum)
            {
                testList.Add(addNum, addNum);

                // test GetFrequency where the list has some elements
                for (int testNum = 1; testNum <= testTo; ++testNum)
                {
                    if (testNum <= addNum)
                    {
                        Assert.AreEqual(testNum, testList.GetFrequency(testNum));
                    }
                    else
                    {
                        Assert.AreEqual(0, testList.GetFrequency(testNum));
                    }
                }
            }

            // Fill in the list again
            for (int addNum = 1; addNum <= fillTo; ++addNum)
            {
                testList.Add(addNum, addNum);

                // test GetFrequency where the list has some elements
                for (int testNum = 1; testNum <= testTo; ++testNum)
                {
                    if (testNum <= addNum)
                    {
                        // Since we're adding the same values twice, the number should be double
                        Assert.AreEqual((testNum * 2), testList.GetFrequency(testNum));
                    }
                    else if (testNum <= fillTo)
                    {
                        // Value hasn't been incremented yet, test old value
                        Assert.AreEqual(testNum, testList.GetFrequency(testNum));
                    }
                    else
                    {
                        Assert.AreEqual(0, testList.GetFrequency(testNum));
                    }
                }
            }

            // Remove elements from the list
            for (int removeNum = fillTo; removeNum >= 1; --removeNum)
            {
                testList.Remove(removeNum, removeNum);

                // test GetFrequency where the list has some elements
                for (int testNum = 1; testNum <= testTo; ++testNum)
                {
                    if (testNum < removeNum)
                    {
                        // Value hasn't been decremented yet, test old value
                        Assert.AreEqual((testNum * 2), testList.GetFrequency(testNum));
                    }
                    else if (testNum <= fillTo)
                    {
                        // Removed value should return back
                        Assert.AreEqual(testNum, testList.GetFrequency(testNum));
                    }
                    else
                    {
                        Assert.AreEqual(0, testList.GetFrequency(testNum));
                    }
                }
            }

            // Remove elements from the list again
            for (int removeNum = fillTo; removeNum >= 1; --removeNum)
            {
                testList.Remove(removeNum, removeNum);

                // test GetFrequency where the list has some elements
                for (int testNum = 1; testNum <= testTo; ++testNum)
                {
                    if (testNum < removeNum)
                    {
                        Assert.AreEqual(testNum, testList.GetFrequency(testNum));
                    }
                    else
                    {
                        Assert.AreEqual(0, testList.GetFrequency(testNum));
                    }
                }
            }
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
            // FIXME: test CopyTo(T[], int) under edge cases
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
            // FIXME: test CopyTo(RandomList[], int) under edge cases
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
            if (expectedComparer != null)
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
