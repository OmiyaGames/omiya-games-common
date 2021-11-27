using System.Collections;
using NUnit.Framework;
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
		/// <summary>
		/// A Test behaves as an ordinary method
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

			// Test exceptions
			Assert.Throws<System.ArgumentException>(() =>
			{
				history = new UndoHistory(0);
			});
			Assert.Throws<System.ArgumentException>(() =>
			{
				history = new UndoHistory(-1);
			});

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
	}
}
