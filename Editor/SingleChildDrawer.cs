using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="SingleChildDrawer.cs" company="Omiya Games">
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
	/// <strong>Date:</strong> 2/6/2022<br/>
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
	/// Helper <see cref="PropertyDrawer"/> for drawing any classes that has only
	/// a single child property in the inspector.  It will use Unity's default
	/// method of drawing said variable.
	/// </summary>
	public abstract class SingleChildDrawer : PropertyDrawer
	{
		/// <summary>
		/// Name of the single serialized variable.
		/// </summary>
		public abstract string SerializedVariableName { get; }

		/// <inheritdoc/>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// Using PropertyScope on the parent property means that
			// prefab override logic works on the entire property.
			using (var scope = new EditorGUI.PropertyScope(position, label, property))
			{
				// Draw the child field
				EditorGUI.PropertyField(position, ChildProperty(property), label);
			}
		}

		/// <inheritdoc/>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(ChildProperty(property), label);
		}

		/// <summary>
		/// Gets the single serialized variable's property
		/// from the parent property.
		/// </summary>
		/// <param name="property">
		/// Parent property (source of this editor.)
		/// </param>
		/// <returns>
		/// Child property.
		/// </returns>
		protected SerializedProperty ChildProperty(SerializedProperty property) => property.FindPropertyRelative(SerializedVariableName);
	}

}
