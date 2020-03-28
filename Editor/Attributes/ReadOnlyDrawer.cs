using UnityEngine;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <copyright file="ReadOnlyDrawer.cs">
    /// Code by It3ration from Unity Answers:
    /// http://answers.unity3d.com/questions/489942/how-to-make-a-readonly-property-in-inspector.html
    /// </copyright>
    /// <author>It3ration</author>
    /// <author>Taro Omiya</author>
    /// <date>10/1/2014</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Makes a field read-only in the Unity editor with <code>[ReadOnly]</code>.
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
    ///     <description>10/1/2014</description>
    ///     <description>It3ration</description>
    ///     <description>Initial verison</description>
    ///   </item>
    ///   <item>
    ///     <description>3/25/2020</description>
    ///     <description>Taro Omiya</description>
    ///     <description>Converted the class to a package</description>
    ///   </item>
    /// </list>
    /// </remarks>
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
