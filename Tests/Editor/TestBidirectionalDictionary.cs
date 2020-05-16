using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.Keys"/>, <see cref="BidirectionalDictionary{KEY, VALUE}.Values"/> and  <see cref="BidirectionalDictionary{KEY, VALUE}.Count"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Keys"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Values"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Count"/>
        [Test]
        public void TestKeysValuesCountProperties()
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

        // TODO: test this[] property
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

        #region Test Add
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.GetValue(KEY)"/>, <see cref="BidirectionalDictionary{KEY, VALUE}.GetKey(VALUE)"/>, <see cref="BidirectionalDictionary{KEY, VALUE}.TryGetValue(KEY, out VALUE)"/>, and <see cref="BidirectionalDictionary{KEY, VALUE}.TryGetKey(VALUE, out KEY)"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.GetValue(KEY)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.GetKey(VALUE)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.TryGetValue(KEY, out VALUE)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.TryGetKey(VALUE, out KEY)"/>
        [Test]
        public void TestGetKeyValueEdgeCases()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<string, string> testEdgeCase = new BidirectionalDictionary<string, string>();
            string test;

            // Test null pointer exception
            Assert.Throws<ArgumentNullException>(delegate
            {
                test = testEdgeCase[null];
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.GetValue(null);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.GetKey(null);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.TryGetValue(null, out test);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.TryGetKey(null, out test);
            });

            // Test non-existant key and value
            Assert.Throws<KeyNotFoundException>(delegate
            {
                test = testEdgeCase[""];
            });
            Assert.Throws<KeyNotFoundException>(delegate
            {
                testEdgeCase.GetValue("");
            });
            Assert.Throws<KeyNotFoundException>(delegate
            {
                testEdgeCase.GetKey("");
            });
            Assert.IsFalse(testEdgeCase.TryGetValue("", out test));
            Assert.AreEqual(default(string), test);
            Assert.IsFalse(testEdgeCase.TryGetKey("", out test));
            Assert.AreEqual(default(string), test);
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.Add(KEY, VALUE)"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Add(KEY, VALUE)"/>
        [Test]
        public void TestAddEdgeCases()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<string, string> testEdgeCase = new BidirectionalDictionary<string, string>();
            string test = "Valid String";

            // Test null pointer exception
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.Add(null, test);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.Add(test, null);
            });
        }

        /// <summary>
        /// Unit test for:<br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.GetValue(KEY)"/>,<br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.GetKey(VALUE)"/>,<br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.TryGetValue(KEY, out VALUE)"/>,<br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.TryGetKey(VALUE, out KEY)"/>,<br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.Contains(KeyValuePair{KEY, VALUE})"/>,<br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.ContainsKey(KEY)"/>,and <br/>
        /// <see cref="BidirectionalDictionary{KEY, VALUE}.ContainsValue(VALUE)"/>
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.GetValue(KEY)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.GetKey(VALUE)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.TryGetValue(KEY, out VALUE)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.TryGetKey(VALUE, out KEY)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.Contains(KeyValuePair{KEY, VALUE})"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.ContainsKey(KEY)"/>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.ContainsValue(VALUE)"/>
        [Test]
        public void TestAddGetTryGetContains()
        {
            const int nonExistantKey = 9999;
            const string nonExistantValue = "Random";

            // Create an empty bidirectional dictionary
            Dictionary<int, string> expectedResults = new Dictionary<int, string>();
            BidirectionalDictionary<int, string> testDefault = new BidirectionalDictionary<int, string>();
            for (int currentKey = 0; currentKey <= 5; ++currentKey)
            {
                // Add a pair into the dictionary
                string currentValue = currentKey.ToString();
                expectedResults.Add(currentKey, currentValue);

                // Test both adds
                if ((currentKey % 2) > 0)
                {
                    testDefault.Add(currentKey, currentValue);
                }
                else
                {
                    testDefault.Add(new KeyValuePair<int, string>(currentKey, currentValue));
                }

                // Confirm size
                Assert.AreEqual((currentKey + 1), testDefault.Count);

                // Confirm content
                VerifyContent(expectedResults, testDefault);

                // Confirm getters and setters
                int testKey;
                string testValue;
                for (int expectedKey = 0; expectedKey <= 5; ++expectedKey)
                {
                    // Test whether key-value exists
                    string expectedValue = expectedKey.ToString();
                    if (expectedKey <= currentKey)
                    {
                        // Test existing key and value
                        Assert.AreEqual(expectedValue, testDefault[expectedKey]);
                        Assert.AreEqual(expectedValue, testDefault.GetValue(expectedKey));
                        Assert.AreEqual(expectedKey, testDefault.GetKey(expectedValue));

                        Assert.IsTrue(testDefault.TryGetValue(expectedKey, out testValue));
                        Assert.AreEqual(expectedValue, testValue);
                        Assert.IsTrue(testDefault.TryGetKey(expectedValue, out testKey));
                        Assert.AreEqual(expectedKey, testKey);

                        Assert.IsTrue(testDefault.ContainsKey(expectedKey));
                        Assert.IsTrue(testDefault.ContainsValue(expectedValue));
                        Assert.IsTrue(testDefault.Contains(new KeyValuePair<int, string>(expectedKey, expectedValue)));

                        // Test edge cases for contains
                        Assert.IsFalse(testDefault.Contains(new KeyValuePair<int, string>(expectedKey, nonExistantValue)));
                        Assert.IsFalse(testDefault.Contains(new KeyValuePair<int, string>(nonExistantKey, expectedValue)));

                        // Test edge case for double-adding
                        Assert.Throws<ArgumentException>(delegate
                        {
                            testDefault.Add(expectedKey, nonExistantValue);
                        });
                        Assert.Throws<ArgumentException>(delegate
                        {
                            testDefault.Add(nonExistantKey, expectedValue);
                        });
                        Assert.Throws<ArgumentException>(delegate
                        {
                            testDefault.Add(new KeyValuePair<int, string>(expectedKey, nonExistantValue));
                        });
                        Assert.Throws<ArgumentException>(delegate
                        {
                            testDefault.Add(new KeyValuePair<int, string>(nonExistantKey, expectedValue));
                        });
                    }
                    else
                    {
                        // Test non-existant key and value
                        Assert.Throws<KeyNotFoundException>(delegate
                        {
                            testValue = testDefault[expectedKey];
                        });
                        Assert.Throws<KeyNotFoundException>(delegate
                        {
                            testDefault.GetValue(expectedKey);
                        });
                        Assert.Throws<KeyNotFoundException>(delegate
                        {
                            testDefault.GetKey(expectedValue);
                        });

                        Assert.IsFalse(testDefault.TryGetValue(expectedKey, out testValue));
                        Assert.AreEqual(default(string), testValue);
                        Assert.IsFalse(testDefault.TryGetKey(expectedValue, out testKey));
                        Assert.AreEqual(default(int), testKey);

                        Assert.IsFalse(testDefault.ContainsKey(expectedKey));
                        Assert.IsFalse(testDefault.ContainsValue(expectedValue));

                        // Test all cases for contains
                        Assert.IsFalse(testDefault.Contains(new KeyValuePair<int, string>(expectedKey, expectedValue)));
                        Assert.IsFalse(testDefault.Contains(new KeyValuePair<int, string>(expectedKey, nonExistantValue)));
                        Assert.IsFalse(testDefault.Contains(new KeyValuePair<int, string>(nonExistantKey, expectedValue)));
                    }
                }
            }
        }
        #endregion

        #region Test Remove
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.RemoveKey(KEY)"/>, <see cref="BidirectionalDictionary{KEY, VALUE}.RemoveValue(VALUE)"/>, <see cref="BidirectionalDictionary{KEY, VALUE}.Remove(KeyValuePair{KEY, VALUE})"/>, and <see cref="BidirectionalDictionary{KEY, VALUE}.Remove(KEY)"/>, focusing on edge cases.
        /// </summary>
        [Test]
        public void TestRemoveEdgeCases()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<string, string> testEdgeCase = new BidirectionalDictionary<string, string>();

            // Test null-pointer error on empty list
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.Remove(null);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.RemoveKey(null);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.RemoveValue(null);
            });

            // Test invalid remove action on empty list
            KeyValuePair<string, string> addPair = new KeyValuePair<string, string>("test", "TEST");
            Assert.IsFalse(testEdgeCase.Remove("test"));
            Assert.IsFalse(testEdgeCase.Remove(addPair));
            Assert.IsFalse(testEdgeCase.RemoveKey("test"));
            Assert.IsFalse(testEdgeCase.RemoveValue("test"));

            // Test null-pointer error on a non-empty list
            testEdgeCase.Add(addPair);
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.Remove(null);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.RemoveKey(null);
            });
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.RemoveValue(null);
            });

            // Test invalid remove action on non-empty list
            Assert.IsFalse(testEdgeCase.Remove(addPair.Value));
            Assert.IsFalse(testEdgeCase.RemoveKey(addPair.Value));
            Assert.IsFalse(testEdgeCase.RemoveValue(addPair.Key));

            // Test every combo of invalid remove action on non-empty list
            KeyValuePair<string, string> dudPair = new KeyValuePair<string, string>(addPair.Value, addPair.Key);
            Assert.IsFalse(testEdgeCase.Remove(dudPair));
            dudPair = new KeyValuePair<string, string>(addPair.Key, addPair.Key);
            Assert.IsFalse(testEdgeCase.Remove(dudPair));
            dudPair = new KeyValuePair<string, string>(addPair.Value, addPair.Value);
            Assert.IsFalse(testEdgeCase.Remove(dudPair));

            // Test remove when the element is already removed
            Assert.IsTrue(testEdgeCase.Remove(addPair.Key));
            Assert.IsFalse(testEdgeCase.Remove(addPair.Key));

            testEdgeCase.Add(addPair);
            Assert.IsTrue(testEdgeCase.Remove(addPair));
            Assert.IsFalse(testEdgeCase.Remove(addPair));

            testEdgeCase.Add(addPair);
            Assert.IsTrue(testEdgeCase.RemoveKey(addPair.Key));
            Assert.IsFalse(testEdgeCase.RemoveKey(addPair.Key));

            testEdgeCase.Add(addPair);
            Assert.IsTrue(testEdgeCase.RemoveValue(addPair.Value));
            Assert.IsFalse(testEdgeCase.RemoveValue(addPair.Value));
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.RemoveKey(KEY)"/> focusing on normal use cases.
        /// </summary>
        [Test]
        public void TestRemoveKey()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<int, int> testNormalCase = new BidirectionalDictionary<int, int>();

            // Fill in the dictionary
            for (int index = 0; index < 10; ++index)
            {
                testNormalCase.Add(index, (index * 10));
            }

            // Run the remove functions, and double-check the content of the dictionary is correct
            KeyValuePair<int, int> check = new KeyValuePair<int, int>();
            int expectedCount = testNormalCase.Count;
            Assert.Greater(expectedCount, 0);
            for (int i = 0; i < 10; ++i)
            {
                // Run the remove function
                Assert.IsTrue(testNormalCase.RemoveKey(i));

                // Verify dictionary size
                --expectedCount;
                Assert.AreEqual(expectedCount, testNormalCase.Count);

                // Verify content of the dictionary
                for (int j = 0; j < 10; ++j)
                {
                    check = new KeyValuePair<int, int>(j, (j * 10));
                    Assert.AreEqual((j > i), testNormalCase.Contains(check));
                }
            }
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.RemoveValue(Value)"/> focusing on normal use cases.
        /// </summary>
        [Test]
        public void TestRemoveValue()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<int, string> testNormalCase = new BidirectionalDictionary<int, string>();

            // Fill in the dictionary
            for (int index = 0; index < 10; ++index)
            {
                testNormalCase.Add(index, index.ToString());
            }

            // Run the remove functions, and double-check the content of the dictionary is correct
            KeyValuePair<int, string> check;
            int expectedCount = testNormalCase.Count;
            Assert.Greater(expectedCount, 0);
            for (int i = 0; i < 10; ++i)
            {
                // Run the remove function
                Assert.IsTrue(testNormalCase.RemoveValue(i.ToString()));

                // Verify dictionary size
                --expectedCount;
                Assert.AreEqual(expectedCount, testNormalCase.Count);

                // Verify content of the dictionary
                for (int j = 0; j < 10; ++j)
                {
                    check = new KeyValuePair<int, string>(j, j.ToString());
                    Assert.AreEqual((j > i), testNormalCase.Contains(check));
                }
            }
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.Remove(KeyValuePair{KEY, VALUE})"/> and <see cref="BidirectionalDictionary{KEY, VALUE}.Remove(KEY)"/>, focusing on normal use cases.
        /// </summary>
        [Test]
        public void TestRemove()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<int, int> testNormalCase = new BidirectionalDictionary<int, int>();

            // Fill in the dictionary
            for (int index = 0; index < 10; ++index)
            {
                testNormalCase.Add(index, (index * 10));
            }

            // Run the remove functions, and double-check the content of the dictionary is correct
            KeyValuePair<int, int> check = new KeyValuePair<int, int>();
            int expectedCount = testNormalCase.Count;
            Assert.Greater(expectedCount, 0);
            for (int i = 9; i >= 0; --i)
            {
                // Run the remove function
                Assert.IsTrue(testNormalCase.Remove(i));

                // Verify dictionary size
                --expectedCount;
                Assert.AreEqual(expectedCount, testNormalCase.Count);

                // Verify content of the dictionary
                for (int j = 0; j < 10; ++j)
                {
                    check = new KeyValuePair<int, int>(j, (j * 10));
                    Assert.AreEqual((j < i), testNormalCase.Contains(check));
                }
            }

            // Fill in the dictionary
            for (int index = 0; index < 10; ++index)
            {
                testNormalCase.Add(index, (index * 10));
            }

            // Run the remove functions, and double-check the content of the dictionary is correct
            expectedCount = testNormalCase.Count;
            Assert.Greater(expectedCount, 0);
            for (int i = 9; i >= 0; --i)
            {
                // Run the remove function
                check = new KeyValuePair<int, int>(i, (i * 10));
                Assert.IsTrue(testNormalCase.Remove(check));

                // Verify dictionary size
                --expectedCount;
                Assert.AreEqual(expectedCount, testNormalCase.Count);

                // Verify content of the dictionary
                for (int j = 0; j < 10; ++j)
                {
                    check = new KeyValuePair<int, int>(j, (j * 10));
                    Assert.AreEqual((j < i), testNormalCase.Contains(check));
                }
            }
        }
        #endregion

        #region Test Set
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.SetKey(VALUE, KEY)"/> and <see cref="BidirectionalDictionary{KEY, VALUE}.SetValue(KEY, VALUE)"/>, focusing on edge cases.
        /// </summary>
        [Test]
        public void TestSetEdgeCases()
        {
            // TODO: e.g. Set
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.SetKey(VALUE, KEY)"/> and <see cref="BidirectionalDictionary{KEY, VALUE}.SetValue(KEY, VALUE)"/>, focusing on normal use cases.
        /// </summary>
        [Test]
        public void TestSet()
        {
            // TODO: e.g. Set
        }
        #endregion

        #region Test CopyTo
        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.CopyTo(KeyValuePair{KEY, VALUE}[], int)"/>, focusing on edge cases.
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.CopyTo(KeyValuePair{KEY, VALUE}[], int)"/>
        [Test]
        public void TestCopyToEdgeCases()
        {
            // Create an empty bidirectional dictionary
            BidirectionalDictionary<string, string> testEdgeCase = new BidirectionalDictionary<string, string>();
            KeyValuePair<string, string>[] copyToArray = null;
            KeyValuePair<string, string> addPair = new KeyValuePair<string, string>("Test", "test");

            // Test null pointer exception
            testEdgeCase.Add(addPair);
            Assert.Throws<ArgumentNullException>(delegate
            {
                testEdgeCase.CopyTo(copyToArray, 0);
            });

            // Test no space
            copyToArray = new KeyValuePair<string, string>[0];
            Assert.Throws<ArgumentException>(delegate
            {
                testEdgeCase.CopyTo(copyToArray, 0);
            });
            Assert.Throws<ArgumentOutOfRangeException>(delegate
            {
                testEdgeCase.CopyTo(copyToArray, 1);
            });
            copyToArray = new KeyValuePair<string, string>[1];
            Assert.Throws<ArgumentException>(delegate
            {
                testEdgeCase.CopyTo(copyToArray, 1);
            });

            // Test valid case
            Assert.DoesNotThrow(delegate
            {
                testEdgeCase.CopyTo(copyToArray, 0);
            });
            Assert.AreEqual(copyToArray[0], addPair);

            // Test if there's nothing to copy over
            addPair = new KeyValuePair<string, string>();
            testEdgeCase.Clear();
            copyToArray[0] = addPair;
            Assert.DoesNotThrow(delegate
            {
                testEdgeCase.CopyTo(copyToArray, 0);
            });
            Assert.AreEqual(copyToArray[0], addPair);
        }

        /// <summary>
        /// Unit test for <see cref="BidirectionalDictionary{KEY, VALUE}.CopyTo(KeyValuePair{KEY, VALUE}[], int)"/>, focusing on normal cases.
        /// </summary>
        /// <seealso cref="BidirectionalDictionary{KEY, VALUE}.CopyTo(KeyValuePair{KEY, VALUE}[], int)"/>
        [Test]
        public void TestCopyTo()
        {
            // Create a bidirectional dictionary
            BidirectionalDictionary<int, string> testNormalCase = new BidirectionalDictionary<int, string>();
            for (int index = (default(int) + 10); index <= (default(int) + 30); index += 5)
            {
                testNormalCase.Add(index, index.ToString());
            }

            // Do a normal copyTo where array size and dictionary are the same
            KeyValuePair<int, string>[] copyToArray = new KeyValuePair<int, string>[testNormalCase.Count];
            testNormalCase.CopyTo(copyToArray, 0);

            // Verify array content
            HashSet<int> allKeys = new HashSet<int>();
            foreach (KeyValuePair<int, string> pair in copyToArray)
            {
                if (pair.Key != default(int))
                {
                    Assert.IsTrue(testNormalCase.Contains(pair));
                    allKeys.Add(pair.Key);
                }
                else
                {
                    Assert.Fail("CopyTo() had wrong key");
                }
            }

            // Verify number of elements is equal to the dictionary
            Assert.AreEqual(testNormalCase.Count, allKeys.Count);


            // Do a normal copyTo where array size is larger than dictionary, and only the beginning is copied to
            copyToArray = new KeyValuePair<int, string>[testNormalCase.Count * 2];
            testNormalCase.CopyTo(copyToArray, 0);

            // Verify array content
            allKeys.Clear();
            int numDefaultItems = 0;
            foreach (KeyValuePair<int, string> pair in copyToArray)
            {
                if (pair.Key != default(int))
                {
                    Assert.IsTrue(testNormalCase.Contains(pair));
                    allKeys.Add(pair.Key);
                }
                else
                {
                    ++numDefaultItems;
                }
            }

            // Verify number of elements is equal to the dictionary
            Assert.AreEqual(testNormalCase.Count, allKeys.Count);
            Assert.AreEqual(testNormalCase.Count, numDefaultItems);

            // Do a normal copyTo where array size is larger than dictionary, and only the end is copied to
            testNormalCase.CopyTo(copyToArray, allKeys.Count);

            // Verify array content
            allKeys.Clear();
            int numConflicts = 0;
            foreach (KeyValuePair<int, string> pair in copyToArray)
            {
                if (pair.Key == default(int))
                {
                    Assert.Fail();
                }
                else if (allKeys.Contains(pair.Key) == true)
                {
                    Assert.IsTrue(testNormalCase.Contains(pair));
                    ++numConflicts;
                }
                else
                {
                    Assert.IsTrue(testNormalCase.Contains(pair));
                    allKeys.Add(pair.Key);
                }
            }

            // Verify number of elements is equal to the dictionary
            Assert.AreEqual(testNormalCase.Count, allKeys.Count);
            Assert.AreEqual(testNormalCase.Count, numConflicts);
        }
        #endregion

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
