using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="EnumFlagsDrawer.cs">
    /// Code by Aqibsadiq from Unity Forums:
    /// https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/
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
    /// Makes an enum multi-selectable in the Unity editor with [EnumFlags].
    /// </summary>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsDrawer : PropertyDrawer
    {
        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            property.intValue = DisplayEnumFlags(position, property, label);

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Draws a multi-selectable enum field in the inspector.
        /// <seealso cref="DisplayEnumFlags(Rect, SerializedProperty, GUIContent, string[])"/>
        /// </summary>
        /// <param name="position">The area to draw.</param>
        /// <param name="property">The property to draw.</param>
        /// <param name="label">Label of the field, if any.</param>
        /// <returns>Selected bit-flags.</returns>
        public static int DisplayEnumFlags(Rect position, SerializedProperty property, GUIContent label)
        {
            return DisplayEnumFlags(position, property, label, property.enumNames);
        }

        /// <summary>
        /// Draws a multi-selectable enum field in the inspector.
        /// <seealso cref="EditorGUI.MaskField(Rect, GUIContent, int, string[])"/>
        /// </summary>
        /// <param name="position">The area to draw.</param>
        /// <param name="property">The property to draw.</param>
        /// <param name="label">Label of the field, if any.</param>
        /// <param name="enumNames">List of display names for each enum flag.</param>
        /// <returns>Selected bit-flags.</returns>
        public static int DisplayEnumFlags(Rect position, SerializedProperty property, GUIContent label, string[] enumNames)
        {
            return EditorGUI.MaskField(position, label, property.intValue, enumNames);
        }
    }
}
