using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="TrackableDrawer.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2021 Omiya Games
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
    /// <strong>Date:</strong> 6/28/2021<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Converted the class to a package.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Property Drawer for <see cref="Trackable{T}}"/> that draws the value directly.
    /// </summary>
    [CustomPropertyDrawer(typeof(Trackable<>))]
    public class TrackableDrawer : PropertyDrawer
    {
        bool includeChildren = false;

        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using PropertyScope on the parent property means that
            // prefab override logic works on the entire property.
            using (var propertyScope = new EditorGUI.PropertyScope(position, label, property))
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                // Cache the old value
                SerializedProperty childProperty = property.FindPropertyRelative("value");
                object oldValue = property.serializedObject.targetObject;

                // Draw the value as a typical property
                includeChildren = EditorGUI.PropertyField(position, childProperty, label, includeChildren);

                // Check if the child property changed
                if(changeScope.changed)
                {
                    // Notify the object value has changed
                    var trackable = (IEditorTrackable)property.serializedObject.targetObject;
                    trackable.OnValueChangedInEditor(oldValue, childProperty.serializedObject.targetObject);
                }
            }
        }

        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("value"), label, includeChildren);
        }
    }
}
