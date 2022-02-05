using System;
using System.Collections.Generic;
using UnityEngine;

namespace OmiyaGames
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="SerializableListSet.cs" company="Omiya Games">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2022 Omiya Games
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
	/// <strong>Date:</strong> 2/5/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>
	/// Initial version.
	/// </description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// A serializable <seealso cref="ListSet{T}"/>. Expose it on the inspector
	/// like a normal list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class SerializableListSet<T> : ListSet<T>, ISerializationCallbackReceiver
	{
		const int MIN_CAPACITY = 2;

		[SerializeField]
		List<T> serializedList;
		[SerializeField, HideInInspector]
		bool isSerializing = false;

		public SerializableListSet()
		{
			serializedList = new List<T>();
		}

		public SerializableListSet(int capacity) : base(capacity)
		{
			serializedList = new List<T>(capacity);
		}

		public SerializableListSet(IEqualityComparer<T> comparer) : base(comparer)
		{
			serializedList = new List<T>();
		}

		public SerializableListSet(int capacity, IEqualityComparer<T> comparer) : base(capacity, comparer)
		{
			serializedList = new List<T>(capacity);
		}

		public bool IsSerializing => isSerializing;
		public IReadOnlyList<T> SerializedList => serializedList.AsReadOnly();


		[Obsolete("Manual call not supported.", true)]
		public void OnBeforeSerialize()
		{
			// Indicate we started serializing
			isSerializing = true;

			// Clear serializedList content
			ClearSerializedList();

			// Populate the list
			foreach (T item in this)
			{
				serializedList.Add(item);
			}
		}

		[Obsolete("Manual call not supported.", true)]
		public void OnAfterDeserialize()
		{
			// Clear this HashSet's contents
			Clear();

			if (serializedList != null)
			{
				// Populate this HashSet
				foreach (T item in serializedList)
				{
					Add(item);
				}
			}

			// Indicate we're done serializing
			isSerializing = false;

			// Reset the serialization
			ClearSerializedList();
		}

		void ClearSerializedList()
		{
			int capacity = Mathf.Max(Count, MIN_CAPACITY);
			if (serializedList == null)
			{
				serializedList = new List<T>(capacity);
			}
			else
			{
				serializedList.Clear();
				serializedList.Capacity = capacity;
			}
		}
	}
}
