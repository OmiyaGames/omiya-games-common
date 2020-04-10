using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OmiyaGames.Common.Runtime.Tests
{
    ///-----------------------------------------------------------------------
    /// <copyright file="TestBidirectionalDictionary.cs" company="Omiya Games">
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
    /// Unit test script for <see cref="BidirectionalDictionary{KEY, VALUE}"/>.
    /// </summary>
    /// 
    /// <seealso cref="BidirectionalDictionary{KEY, VALUE}"/>
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
    public class TestBidirectionalDictionary
    {
        /// <summary>
        /// A <see cref="SingleDigitEqualityComparer"/> for testing purposes.
        /// </summary>
        readonly IEqualityComparer<int> testComparer = new SingleDigitEqualityComparer();

        #region Test Constructors
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary"/>
        [Test]
        public void TestConstructorDefault()
        {
            // Test the default constructor, and whether it creates an empty dictionary
            BidirectionalDictionary<int, string> testDictionary = new BidirectionalDictionary<int, string>();
            Assert.IsNotNull(testDictionary);
            Assert.AreEqual(0, testDictionary.Count);
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(int)"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(int)"/>
        [Test]
        public void TestConstructorInt()
        {
            // Test the capacity constructors, and whether it creates an empty dictionary
            BidirectionalDictionary<int, string> testDictionary;
            for (int capacity = 10; capacity <= 30; capacity += 10)
            {
                testDictionary = new BidirectionalDictionary<int, string>(capacity);
                Assert.IsNotNull(testDictionary);
                Assert.AreEqual(0, testDictionary.Count);

                // Interestingly, I can't get the capacity info from the member variables.
            }
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(IEqualityComparer{KEY}, IEqualityComparer{VALUE})"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(IEqualityComparer{KEY}, IEqualityComparer{VALUE})"/>
        [Test]
        public void TestConstructorIEqualityComparers()
        {
            // Test the constructor, and whether it creates an empty dictionary
            BidirectionalDictionary<int, int> testDictionary = new BidirectionalDictionary<int, int>(testComparer, testComparer);
            Assert.IsNotNull(testDictionary);
            Assert.AreEqual(0, testDictionary.Count);
            Assert.AreEqual(testComparer, testDictionary.KeyComparer);
            Assert.AreEqual(testComparer, testDictionary.ValueComparer);
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(int, IEqualityComparer{KEY}, IEqualityComparer{VALUE})"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(int, IEqualityComparer{KEY}, IEqualityComparer{VALUE})"/>
        [Test]
        public void TestConstructorIntIEqualityComparers()
        {
            // Test the capacity constructors, and whether it creates an empty dictionary
            BidirectionalDictionary<int, int> testDictionary;
            for (int capacity = 10; capacity <= 30; capacity += 10)
            {
                testDictionary = new BidirectionalDictionary<int, int>(capacity, testComparer, testComparer);
                Assert.IsNotNull(testDictionary);
                Assert.AreEqual(0, testDictionary.Count);
                Assert.AreEqual(testComparer, testDictionary.KeyComparer);
                Assert.AreEqual(testComparer, testDictionary.ValueComparer);
            }
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(Dictionary{KEY, VALUE})"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(Dictionary{KEY, VALUE})"/>
        [Test]
        public void TestConstructorDictionary()
        {
            // Setup test data
            Dictionary<int, string> referenceMap = new Dictionary<int, string>(testComparer);
            for (int i = 0; i < 5; ++i)
            {
                referenceMap.Add(i, i.ToString());
            }

            // Test the constructor, and whether it copied the dictionary's content
            BidirectionalDictionary<int, string> testDictionary = new BidirectionalDictionary<int, string>(referenceMap);
            Assert.IsNotNull(testDictionary);
            Assert.AreEqual(testComparer, testDictionary.KeyComparer);
            Assert.AreNotEqual(testComparer, testDictionary.ValueComparer);

            // Also verify the content is correct
            VerifyContent(referenceMap, testDictionary);
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(BidirectionalDictionary{KEY, VALUE})"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(BidirectionalDictionary{KEY, VALUE})"/>
        [Test]
        public void TestConstructorBidirectionalDictionary()
        {
            // Setup test data
            BidirectionalDictionary<int, int> referenceMap = new BidirectionalDictionary<int, int>(testComparer, testComparer);
            for (int i = 0; i < 5; ++i)
            {
                referenceMap.Add(i, (i + (i * 10)));
            }

            // Test the constructor, and whether it copied the dictionary's content
            BidirectionalDictionary<int, int> testDictionary = new BidirectionalDictionary<int, int>(referenceMap);
            Assert.IsNotNull(testDictionary);
            Assert.AreEqual(testComparer, testDictionary.KeyComparer);
            Assert.AreEqual(testComparer, testDictionary.ValueComparer);

            // Also verify the content is correct
            VerifyContent(referenceMap, testDictionary);
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(IDictionary{KEY, VALUE}, IEqualityComparer{KEY}, IEqualityComparer{VALUE})"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.BidirectionalDictionary(IDictionary{KEY, VALUE}, IEqualityComparer{KEY}, IEqualityComparer{VALUE})"/>
        [Test]
        public void TestConstructorIDictionaryIEqualityComparers()
        {
            // Setup test data
            IDictionary<int, int> referenceMap = new BidirectionalDictionary<int, int>(testComparer, testComparer);
            for (int i = 0; i < 5; ++i)
            {
                referenceMap.Add(i, (i + (i * 10)));
            }

            // Test the constructor, and whether it copied the dictionary's content
            BidirectionalDictionary<int, int> testDictionary = new BidirectionalDictionary<int, int>(referenceMap, testComparer, testComparer);
            Assert.IsNotNull(testDictionary);
            Assert.AreEqual(testComparer, testDictionary.KeyComparer);
            Assert.AreEqual(testComparer, testDictionary.ValueComparer);

            // Also verify the content is correct
            VerifyContent(referenceMap, testDictionary);
        }
        #endregion

        #region Test Properties
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.Keys"/> and <see cref="BidirectionalDictionary{KEY, VALUE}.Values"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Keys"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Values"/>
        [Test]
        public void TestKeysValuesProperties()
        {
            // Test edge case of an empty dictionary
            BidirectionalDictionary<int, string> testDictionary = new BidirectionalDictionary<int, string>();

            // Test the keys
            ICollection<int> testKeys = testDictionary.Keys;
            Assert.AreEqual(0, testKeys.Count);
            foreach (int key in testKeys)
            {
                Assert.Fail("Not expecting any keys!");
            }

            // Test the Values
            ICollection<string> testValues = testDictionary.Values;
            Assert.AreEqual(0, testValues.Count);
            foreach (string value in testValues)
            {
                Assert.Fail("Not expecting any values!");
            }

            // Setup test data
            Dictionary<int, string> referenceMap = new Dictionary<int, string>(testComparer);
            HashSet<int> expectedKeys = new HashSet<int>();
            HashSet<string> expectedValues = new HashSet<string>();
            for (int i = 0; i < 5; ++i)
            {
                referenceMap.Add(i, i.ToString());
                expectedKeys.Add(i);
                expectedValues.Add(referenceMap[i]);
            }

            // Populate a bidirectional dictionary
            testDictionary = new BidirectionalDictionary<int, string>(referenceMap);

            // Test the keys
            testKeys = testDictionary.Keys;
            Assert.AreEqual(expectedKeys.Count, testKeys.Count);
            foreach (int key in testKeys)
            {
                Assert.IsTrue(expectedKeys.Contains(key));
            }

            // Test the Values
            testValues = testDictionary.Values;
            Assert.AreEqual(expectedValues.Count, testValues.Count);
            foreach (string value in testValues)
            {
                Assert.IsTrue(expectedValues.Contains(value));
            }
        }
        #endregion

        #region Test GetEnumerator
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.GetEnumerator()"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.GetEnumerator()"/>
        [Test]
        public void TestGetEnumerator()
        {
            // Populate a bidirectional dictionary
            BidirectionalDictionary<int, string> testDictionary = new BidirectionalDictionary<int, string>();

            // Test enumerator edge case
            IEnumerator<KeyValuePair<int, string>> enumerator = testDictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Assert.Fail("Should not enumerate in an empty dictionary!");
            }
            foreach (KeyValuePair<int, string> pair in testDictionary)
            {
                Assert.Fail("Should not enumerate in an empty dictionary!");
            }

            // Setup test data
            Dictionary<int, string> referenceMap = new Dictionary<int, string>();
            for (int i = 0; i < 5; ++i)
            {
                referenceMap.Add(i, i.ToString());
            }

            // Test the enumerator
            testDictionary = new BidirectionalDictionary<int, string>(referenceMap);
            enumerator = testDictionary.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Assert.IsTrue(referenceMap.ContainsKey(enumerator.Current.Key));
                Assert.AreEqual(referenceMap[enumerator.Current.Key], enumerator.Current.Value);
            }
            foreach (KeyValuePair<int, string> pair in testDictionary)
            {
                Assert.IsTrue(referenceMap.ContainsKey(pair.Key));
                Assert.AreEqual(referenceMap[pair.Key], pair.Value);
            }
            Debug.Log(testDictionary.ToString());
        }
        #endregion

        #region Test Clear
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.Clear()"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Clear()"/>
        [Test]
        public void TestClear()
        {
            // Setup test data
            Dictionary<int, string> referenceMap = new Dictionary<int, string>();
            for (int i = 0; i < 5; ++i)
            {
                referenceMap.Add(i, i.ToString());
            }

            // Populate a bidirectional dictionary
            BidirectionalDictionary<int, string> testDictionary = new BidirectionalDictionary<int, string>();

            // Test clear
            testDictionary.Clear();
            Assert.AreEqual(0, testDictionary.Count);

            // Test the keys
            ICollection<int> testKeys = testDictionary.Keys;
            Assert.AreEqual(0, testKeys.Count);
            foreach (int key in testKeys)
            {
                Assert.Fail("Not expecting any keys!");
            }

            // Test the Values
            ICollection<string> testValues = testDictionary.Values;
            Assert.AreEqual(0, testValues.Count);
            foreach (string value in testValues)
            {
                Assert.Fail("Not expecting any values!");
            }

            // Test enumerator
            foreach (KeyValuePair<int, string> pair in testDictionary)
            {
                Assert.Fail("Not expecting to be able to enumerate a cleared dictionary!");
            }
        }
        #endregion

        // TODO: test the rest of the methods

        #region Helper Methods
        private static void VerifyContent<KEY, VALUE>(IDictionary<KEY, VALUE> expectedResults, BidirectionalDictionary<KEY, VALUE> testDictionary)
        {
            KEY testKey;
            VALUE testValue;
            Assert.AreEqual(expectedResults.Count, testDictionary.Count);
            foreach (KeyValuePair<KEY, VALUE> expectedPair in expectedResults)
            {
                // Grab the value from key
                Assert.IsTrue(testDictionary.TryGetValue(expectedPair.Key, out testValue));

                // Verify it matches the reference's value
                Assert.AreEqual(expectedPair.Value, testValue);

                // Grab the key from value
                Assert.IsTrue(testDictionary.TryGetKey(expectedPair.Value, out testKey));

                // Verify it matches the reference's value
                Assert.AreEqual(expectedPair.Key, testKey);
            }
        }
        #endregion
    }
}
