using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <copyright file="EnumFlagsDrawer.cs">
    /// Code by Aqibsadiq from Unity Forums:
    /// https://forum.unity.com/threads/multiple-enum-select-from-inspector.184729/
    /// </copyright>
    /// <author>Aqibsadiq</author>
    /// <author>Taro Omiya</author>
    /// <date>11/20/2017</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Makes an enum multi-selectable in the Unity editor with <code>[EnumFlags]</code>.
    /// </summary>
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    ///   <listheader>
    ///     <description>Date</description>
    ///     <description>Name</description>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <description>6/26/2018</description>
    ///     <description>Taro</description>
    ///     <description>Initial version</description>
    ///   </item>
    ///   <item>
    ///     <description>3/25/2020</description>
    ///     <description>Taro</description>
    ///     <description>Converted the class to a package</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
    public class EnumFlagsDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            property.intValue = DisplayEnumFlags(position, property, label);

            EditorGUI.EndProperty();
        }

        public static int DisplayEnumFlags(Rect position, SerializedProperty property, GUIContent label)
        {
            return DisplayEnumFlags(position, property, label, property.enumNames);
        }

        public static int DisplayEnumFlags(Rect position, SerializedProperty property, GUIContent label, string[] enumNames)
        {
            return EditorGUI.MaskField(position, label, property.intValue, enumNames);
        }
    }
}
