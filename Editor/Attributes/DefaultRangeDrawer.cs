﻿using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="DefaultRangeDrawer.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2020 Omiya Games
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
    /// <strong>Date:</strong> 6/26/2018<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.0-preview.1<br/>
    /// <strong>Date:</strong> 3/25/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package.
    /// </description>
    /// </item>
    /// <item>
    /// <term>
    /// <strong>Version:</strong> 0.1.4-preview.1<br/>
    /// <strong>Date:</strong> 5/27/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Updating documentation to be compatible with DocFX.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// <see cref="PropertyDrawer"/> for <see cref="DefaultRangeAttribute"/>.
    /// </summary>
    [CustomPropertyDrawer(typeof(DefaultRangeAttribute))]
    public class DefaultRangeDrawer : IDefaultDrawer
    {
        private bool isEnabled = false;
        private float sliderValue = 0;

        // Draw the property inside the given rect
        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // First get the attribute since it contains the range for the slider
            if (attribute is DefaultRangeAttribute)
            {
                DefaultRangeAttribute range = (DefaultRangeAttribute)attribute;

                // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
                if (property.propertyType == SerializedPropertyType.Float)
                {
                    DisplayCheckboxAndControl(property, range, position, SetToDefaultFloat, DisplayFloatSlider, ref isEnabled, ref sliderValue);
                }
                else if (property.propertyType == SerializedPropertyType.Integer)
                {
                    DisplayCheckboxAndControl(property, range, position, SetToDefaultInt, DisplayIntSlider, ref isEnabled, ref sliderValue);
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, "Use DefaultRange with float or int.");
                }
            }
        }

        /// <summary>
        /// Draws a slider for a <see cref="float"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="range"></param>
        /// <param name="position"></param>
        /// <param name="value"></param>
        static void DisplayFloatSlider(SerializedProperty property, DefaultRangeAttribute range, Rect position, ref float value)
        {
            value = EditorGUI.Slider(position, value, range.Min, range.Max);
        }

        /// <summary>
        /// Draws a slider for an <see cref="int"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="range"></param>
        /// <param name="position"></param>
        /// <param name="value"></param>
        static void DisplayIntSlider(SerializedProperty property, DefaultRangeAttribute range, Rect position, ref float value)
        {
            value = EditorGUI.IntSlider(position, Mathf.RoundToInt(value), Mathf.RoundToInt(range.Min), Mathf.RoundToInt(range.Max));
        }

        /// <summary>
        /// Draws a text box for a <see cref="float"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="range"></param>
        static void SetToDefaultFloat(SerializedProperty property, DefaultRangeAttribute range)
        {
            property.floatValue = range.DefaultNumber;
        }

        /// <summary>
        /// Draws a text box for an <see cref="int"/>.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="range"></param>
        static void SetToDefaultInt(SerializedProperty property, DefaultRangeAttribute range)
        {
            property.floatValue = Mathf.RoundToInt(range.DefaultNumber);
        }
    }
}
