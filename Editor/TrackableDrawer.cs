using System.Collections.Generic;
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
        class FoldoutState
        {
            // TODO: add a mechanism to detect stale dictionary values to later remove
            //public double lastUpdated = EditorApplication.timeSinceStartup;
            public bool isExpanded = false;
        }

        readonly Dictionary<string, FoldoutState> typeToExpandedMap = new Dictionary<string, FoldoutState>();

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
                object oldValue = GetValue(childProperty);

                // Draw the value as a typical property
                FoldoutState state = GetFoldoutState(childProperty);
                state.isExpanded = EditorGUI.PropertyField(position, childProperty, label, state.isExpanded);

                // Check if the child property changed
                if (changeScope.changed)
                {
                    // Notify the object value has changed
                    IEditorTrackable trackable = (IEditorTrackable)fieldInfo.GetValue(property.serializedObject.targetObject);
                    trackable.OnValueChangedInEditor(oldValue, GetValue(childProperty));
                }
            }
        }

        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Check if this property is expanded
            SerializedProperty childProperty = property.FindPropertyRelative("value");
            FoldoutState state = GetFoldoutState(childProperty);
            return EditorGUI.GetPropertyHeight(childProperty, label, state.isExpanded);
        }

        FoldoutState GetFoldoutState(SerializedProperty childProperty)
        {
            if (typeToExpandedMap.TryGetValue(childProperty.type, out FoldoutState state) == false)
            {
                state = new FoldoutState();
                typeToExpandedMap.Add(childProperty.type, state);
            }
            return state;
        }

        object GetValue(SerializedProperty childProperty)
        {
            switch (childProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    return childProperty.intValue;
                case SerializedPropertyType.Float:
                    return childProperty.floatValue;
                case SerializedPropertyType.Enum:
                    return childProperty.enumValueIndex;
                case SerializedPropertyType.Boolean:
                    return childProperty.boolValue;
                case SerializedPropertyType.String:
                    return childProperty.stringValue;
                case SerializedPropertyType.Vector2:
                    return childProperty.vector2Value;
                case SerializedPropertyType.Vector3:
                    return childProperty.vector3Value;
                case SerializedPropertyType.Quaternion:
                    return childProperty.quaternionValue;
                case SerializedPropertyType.Color:
                    return childProperty.colorValue;
                case SerializedPropertyType.Vector2Int:
                    return childProperty.vector2IntValue;
                case SerializedPropertyType.Vector3Int:
                    return childProperty.vector3IntValue;
                case SerializedPropertyType.Vector4:
                    return childProperty.vector4Value;
                case SerializedPropertyType.Rect:
                    return childProperty.rectValue;
                case SerializedPropertyType.Bounds:
                    return childProperty.boundsValue;
                case SerializedPropertyType.RectInt:
                    return childProperty.rectIntValue;
                case SerializedPropertyType.BoundsInt:
                    return childProperty.boundsIntValue;
                case SerializedPropertyType.AnimationCurve:
                    return childProperty.animationCurveValue;
                case SerializedPropertyType.Hash128:
                    return childProperty.hash128Value;
                case SerializedPropertyType.ManagedReference:
                    return childProperty.managedReferenceValue;
                default:
                    return childProperty.objectReferenceValue;
            }
        }

    }
}
