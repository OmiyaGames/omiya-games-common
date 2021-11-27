using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace OmiyaGames.Common.Runtime.Tests
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="TestUndoHistory.cs" company="Omiya Games">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2021 Omiya Games
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
	/// <list type="table">
	/// <listheader>
	/// <term>Revision</term>
	/// <description>Description</description>
	/// </listheader>
	/// <item>
	/// <term>
	/// <strong>Version:</strong> 1.1.0<br/>
	/// <strong>Date:</strong> 11/27/2021<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>Initial version.</description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// Unit test script for <see cref="UndoHistory"/>.
	/// </summary>
	public class TestUndoHistory
	{
		class NumRecord : UndoHistory.IRecord
		{
			public event System.Action<NumRecord, object, UndoHistory> OnAfterRedo;
			public event System.Action<NumRecord, object, UndoHistory> OnAfterUndo;

			public readonly int incrementBy;
			readonly TestUndoHistory parent;

			public NumRecord(TestUndoHistory parent, int incrementBy)
			{
				// Setup member variables
				this.parent = parent;
				this.incrementBy = incrementBy;

				// Perform the action
				OnRedo(null, null);
			}

			public string Description => incrementBy.ToString();

			public void OnRedo(object source, UndoHistory history)
			{
				parent.record += incrementBy;
				OnAfterRedo?.Invoke(this, source, history);
			}

			public void OnUndo(object source, UndoHistory history)
			{
				parent.record -= incrementBy;
				OnAfterUndo?.Invoke(this, source, history);
			}
		}

		struct PastRecord
		{
			public int incrementBy;
			public int record;
		}

		int record;

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory(int)"/>
		/// </summary>
		[Test]
		public void TestConstructor()
		{
			// Test constructor as-is
			UndoHistory history = new UndoHistory();

			// Use the Assert class to test conditions
			Assert.AreEqual(UndoHistory.DefaultCapacity, history.Capacity);
			AssertDefault(history);

			// Test capacity
			for(int size = 1; size <= 10; ++size)
			{
				// Run code
				history = new UndoHistory(size);

				// Test assert
				Assert.AreEqual(size, history.Capacity);
				AssertDefault(history);
			}

			static void AssertDefault(UndoHistory history)
			{
				Assert.Zero(history.Count);
				Assert.IsNull(history.UndoRecord);
				Assert.IsNull(history.RedoRecord);
				Assert.IsFalse(history.CanUndo);
				Assert.IsFalse(history.CanRedo);
				Assert.IsFalse(history.Undo(null));
				Assert.IsFalse(history.Redo(null));
			}
		}

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory(int)"/>
		/// </summary>
		[Test]
		public void TestConstructorExceptions()
		{
			// Test exceptions
			Assert.Throws<System.ArgumentException>(() =>
			{
				UndoHistory history = new UndoHistory(0);
			});
			Assert.Throws<System.ArgumentException>(() =>
			{
				UndoHistory history = new UndoHistory(-1);
			});
		}

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory.Add(UndoHistory.IRecord)"/>
		/// </summary>
		[Test]
		public void TestAdd()
		{
			// Setup
			UndoHistory history = new UndoHistory();
			record = 0;

			// Perform Add
			for(int i = 0; i < 10; ++i)
			{
				// Add record
				NumRecord newRecord = new NumRecord(this, i);
				history.Add(newRecord);

				// Confirm history size
				Assert.AreEqual((i + 1), history.Count);

				// Confirm content
				int index = 0;
				foreach(var oldRecord in history)
				{
					Assert.IsTrue(oldRecord is NumRecord);
					Assert.AreEqual(index, ((NumRecord)oldRecord).incrementBy);
					++index;
				}
			}
		}

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory.Add(UndoHistory.IRecord)"/>
		/// </summary>
		[Test]
		public void TestAddExceptions()
		{
			// Setup
			UndoHistory history = new UndoHistory();
			record = 0;

			// Test exceptions
			Assert.Throws<System.ArgumentNullException>(() =>
			{
				history.Add(null);
			});
		}

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory.Add(UndoHistory.IRecord)"/>
		/// </summary>
		[Test]
		public void TestAddAtCapacity()
		{
			for(int capacity = 1; capacity <= 10; ++capacity)
			{
				// Setup
				UndoHistory history = new UndoHistory(capacity);
				record = 0;

				// Perform Add
				for(int i = 0; i < 10; ++i)
				{
					// Add record
					NumRecord newRecord = new NumRecord(this, i);
					history.Add(newRecord);

					// Confirm history size
					Assert.AreEqual(Mathf.Min(i + 1, capacity), history.Count);

					// Confirm content
					int index = Mathf.Max(0, (i - (capacity - 1)));
					foreach(var oldRecord in history)
					{
						Assert.IsTrue(oldRecord is NumRecord);
						Assert.AreEqual(index, ((NumRecord)oldRecord).incrementBy);
						++index;
					}
				}
			}
		}

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory.Add(UndoHistory.IRecord)"/>
		/// </summary>
		[Test]
		public void TestAddAtMiddle()
		{
			const int MaxSize = 10;
			UndoHistory history = new UndoHistory();
			NumRecord newRecord;
			for(int numUndos = 1; numUndos <= MaxSize; ++numUndos)
			{
				// Setup history
				history.Clear();
				record = 0;
				for(int i = 0; i < MaxSize; ++i)
				{
					// Add record
					newRecord = new NumRecord(this, i);
					history.Add(newRecord);
				}

				// Perform undos
				for(int i = 0; i < numUndos; ++i)
				{
					history.Undo(this);
				}

				// Confirm history size
				Assert.AreEqual(10, history.Count);

				// Add record in middle of history
				newRecord = new NumRecord(this, (MaxSize - numUndos));
				history.Add(newRecord);

				// Confirm history size
				Assert.AreEqual((MaxSize - numUndos + 1), history.Count);

				// Confirm content
				int index = 0;
				foreach(var oldRecord in history)
				{
					Assert.IsTrue(oldRecord is NumRecord);
					Assert.AreEqual(index, ((NumRecord)oldRecord).incrementBy);
					++index;
				}
			}
		}

		/// <summary>
		/// Unit test for <seealso cref="UndoHistory.Undo(object)"/>
		/// </summary>
		[Test]
		public void TestUndoAndRedo()
		{
			// Setup history
			const int MaxSize = 10;
			UndoHistory history = new UndoHistory();
			List<PastRecord> incrementBy = new List<PastRecord>(MaxSize);
			int compareRecord = 0;
			record = 0;

			// Fill with random records
			for(int i = 0; i < MaxSize; ++i)
			{
				// Add record
				int increment = Random.Range(-10, 11);
				history.Add(new NumRecord(this, increment));

				compareRecord += increment;
				incrementBy.Add(new PastRecord()
				{
					incrementBy = increment,
					record = compareRecord
				});
			}

			// Confirm records are the same (just to make sure)
			Assert.AreEqual(compareRecord, record, "Confirming setup worked");
			Assert.AreEqual(MaxSize, history.Count);
			Assert.IsTrue(history.CanUndo);
			Assert.IsFalse(history.CanRedo);

			// Perform undos
			int compareIndex = (incrementBy.Count - 2);
			while(history.CanUndo)
			{
				Assert.IsTrue(history.Undo(this));
				if(compareIndex >= 0)
				{
					Assert.AreEqual(incrementBy[compareIndex].record, record);
				}
				else
				{
					Assert.AreEqual(0, record);
				}

				Assert.AreEqual(MaxSize, history.Count);
				--compareIndex;
			}

			// Confirm can't undo anymore
			Assert.IsFalse(history.Undo(this));
			Assert.IsFalse(history.CanUndo);
			Assert.AreEqual(MaxSize, history.Count);
			Assert.IsTrue(history.CanRedo);

			// Perform redo
			compareIndex = 0;
			while(history.CanRedo)
			{
				Assert.IsTrue(history.Redo(this));
				Assert.AreEqual(incrementBy[compareIndex].record, record);
				Assert.AreEqual(MaxSize, history.Count);
				++compareIndex;
			}

			// Confirm can't redo anymore
			Assert.IsFalse(history.Redo(this));
			Assert.IsFalse(history.CanRedo);
			Assert.AreEqual(MaxSize, history.Count);
			Assert.IsTrue(history.CanUndo);
		}
	}
}
