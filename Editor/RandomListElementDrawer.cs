using UnityEditor;
using UnityEngine;

namespace OmiyaGames.Common.Editor
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="RandomListDrawer.cs" company="Omiya Games">
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
	/// <see cref="PropertyDrawer"/> for <seealso cref="RandomList{T}.ElementFrequency"/>.
	/// </summary>

	[CustomPropertyDrawer(typeof(RandomList<>.ElementFrequency))]
	public class RandomListElementDrawer : PropertyDrawer
	{
		const float ELEMENT_WIDTH_RATIO = 0.65f;
		static readonly GUIContent FREQUENCY_LABEL = new GUIContent("Frequency", "The higher the frequency (relative to the rest of the list,) the more likely this element will be selected.");

		/// <inheritdoc/>
		public override void OnGUI(Rect fullPosition, SerializedProperty property, GUIContent label)
		{
			using (var scope = new EditorGUI.PropertyScope(fullPosition, label, property))
			{
				// Draw the element
				SerializedProperty element = property.FindPropertyRelative("element");
				Rect rect = fullPosition;
				rect.width *= ELEMENT_WIDTH_RATIO;
				EditorGUI.PropertyField(rect, element, label);

				// Calculate the width for the last few elements
				float leftOverWidth = (fullPosition.width - rect.width);
				leftOverWidth /= 2f;

				// Draw the frequency label
				rect.x += rect.width + EditorHelpers.VerticalSpace;
				rect.width = leftOverWidth - EditorHelpers.VerticalSpace;
				EditorGUI.LabelField(rect, FREQUENCY_LABEL);

				SerializedProperty frequency = property.FindPropertyRelative("frequency");
				rect.x += rect.width;
				rect.width = leftOverWidth;
				frequency.intValue = Mathf.Max(EditorGUI.IntField(rect, frequency.intValue), 1);
			}
		}

		/// <inheritdoc/>
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("element"));
		}
	}
}
