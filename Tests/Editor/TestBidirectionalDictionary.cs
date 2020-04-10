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
            Assert.AreEqual(referenceMap.Count, testDictionary.Count);
            Assert.AreEqual(testComparer, testDictionary.KeyComparer);
            Assert.AreNotEqual(testComparer, testDictionary.ValueComparer);

            // Also verify the content is correct
            int testKey;
            string testValue;
            foreach (KeyValuePair<int, string> pair in referenceMap)
            {
                // Grab the value from key
                Assert.IsTrue(testDictionary.TryGetValue(pair.Key, out testValue));

                // Verify it matches the reference's value
                Assert.AreEqual(pair.Value, testValue);

                // Grab the key from value
                Assert.IsTrue(testDictionary.TryGetKey(pair.Value, out testKey));

                // Verify it matches the reference's value
                Assert.AreEqual(pair.Key, testKey);
            }
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
            Assert.AreEqual(referenceMap.Count, testDictionary.Count);
            Assert.AreEqual(testComparer, testDictionary.KeyComparer);
            Assert.AreEqual(testComparer, testDictionary.ValueComparer);

            // Also verify the content is correct
            int testKey, testValue;
            foreach (KeyValuePair<int, int> pair in referenceMap)
            {
                // Grab the value from key
                Assert.IsTrue(testDictionary.TryGetValue(pair.Key, out testValue));

                // Verify it matches the reference's value
                Assert.AreEqual(pair.Value, testValue);

                // Grab the key from value
                Assert.IsTrue(testDictionary.TryGetKey(pair.Value, out testKey));

                // Verify it matches the reference's value
                Assert.AreEqual(pair.Key, testKey);
            }
        }

        // TODO: test the rest of the constructors
        #endregion

        // TODO: test the rest of the methods
    }
}
