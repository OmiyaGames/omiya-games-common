using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="CsvReader.cs" company="Omiya Games">
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
    /// <strong>Date:</strong> 10/12/2018<br/>
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
    /// Property Drawer for <see cref="HsvColor"/> by...just making it into a color picker.
    /// </summary>
    [CustomPropertyDrawer(typeof(HsvColor))]
    public class HsvColorDrawer : PropertyDrawer
    {
        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Grab all values
            SerializedProperty hue = property.FindPropertyRelative("hue");
            SerializedProperty saturation = property.FindPropertyRelative("saturation");
            SerializedProperty value = property.FindPropertyRelative("value");
            SerializedProperty alpha = property.FindPropertyRelative("alpha");

            // Convert these values into a color
            HsvColor color = new HsvColor(hue.floatValue, saturation.floatValue, value.floatValue, alpha.floatValue);
            Color convertedColor = color.ToColor();

            // Draw the color field
            convertedColor = EditorGUI.ColorField(position, label, convertedColor);

            // Convert the color back to the values
            color = HsvColor.FromColor(convertedColor);
            hue.floatValue = color.Hue;
            saturation.floatValue = color.Saturation;
            value.floatValue = color.Value;
            alpha.floatValue = color.Alpha;

            // End the property
            EditorGUI.EndProperty();
        }
    }
}