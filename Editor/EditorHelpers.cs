using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="EditorHelpers.cs" company="Omiya Games">
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
    /// <strong>Date:</strong> 9/20/2018<br/>
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
    /// A series of utilities used throughout the <see cref="OmiyaGames.Common.Editor"/> namespace.
    /// This library focuses on editor-related static functions.
    /// </summary>
    public static class EditorHelpers
    {
        /// <summary>
        /// The minimum help box height on inspector.
        /// </summary>
        public const float MinHelpBoxHeight = 30f;
        /// <summary>
        /// The default vertical margin between controls.
        /// </summary>
        public const float VerticalMargin = 2f;
        /// <summary>
        /// Vertical gap between groups of controls to indicate separation.
        /// </summary>
        public const float VerticalSpace = 8f;
        /// <summary>
        /// Amount of space to indent for embedded controls.
        /// </summary>
        public const float IndentSpace = 14f;

        /// <summary>
        /// Single line height, plus the number of vertical margins
        /// (usually between 0 to 2).
        /// </summary>
        /// <param name="verticalMargin">
        /// Number of margins. 1 for top-only, 2 for top and bottom.
        /// </param>
        /// <returns>Vertical height for single line, plus margins.</returns>
        public static float SingleLineHeight(float verticalMargin)
        {
            return EditorGUIUtility.singleLineHeight + (verticalMargin * 2);
        }

        /// <summary>
        /// Gets total height for an editor.
        /// </summary>
        /// <param name="label">
        /// Label for the control: if it's displayed, increments number of rows.
        /// </param>
        /// <param name="numRows">Number of rows, e.g. number of controls.</param>
        /// <param name="verticalMargin">
        /// The height of each margin between rows.
        /// </param>
        /// <returns>Total height for editor.</returns>
        public static float GetHeight(GUIContent label, int numRows, float verticalMargin = VerticalMargin)
        {
            if ((label != null) && (string.IsNullOrEmpty(label.text) == false))
            {
                numRows += 1;
            }
            return GetHeight(numRows, verticalMargin);
        }

        /// <summary>
        /// Gets total height for an editor.
        /// </summary>
        /// <param name="numRows">Number of rows, e.g. number of controls.</param>
        /// <param name="verticalMargin">
        /// The height of each margin between rows.
        /// </param>
        /// <returns>Total height for editor.</returns>
        public static float GetHeight(int numRows, float verticalMargin = VerticalMargin)
        {
            float height = (EditorGUIUtility.singleLineHeight * numRows);
            height += (verticalMargin * numRows);
            return height;
        }

        /// <summary>
        /// Get the height of the help box, based on the text and width of controls.
        /// </summary>
        /// <param name="text">The text to display in the help box.</param>
        /// <param name="viewWidth">Width of the view.</param>
        /// <param name="minHeight">
        /// The minimum height of help box, if text is short.
        /// </param>
        /// <returns>The height of the help box to display.</returns>
        public static float GetHelpBoxHeight(string text, float viewWidth, float minHeight = MinHelpBoxHeight)
        {
            var content = new GUIContent(text);
            var style = GUI.skin.GetStyle("helpbox");

            return Mathf.Max(minHeight, style.CalcHeight(content, viewWidth));
        }

        /// <summary>
        /// Sets up an <see cref="AnimBool"/>.
        /// </summary>
        /// <param name="editor">
        /// The editor to add the <see cref="AnimBool"/> to.
        /// </param>
        /// <param name="boolAnimation">
        /// The variable to set the new <see cref="AnimBool"/> to.
        /// Note that if it's already set,
        /// that variable will be destroyed first before creating a new one.
        /// </param>
        public static void CreateBool(UnityEditor.Editor editor, ref AnimBool boolAnimation)
        {
            // Destroy the last animation, if any
            DestroyBool(editor, ref boolAnimation);

            // Setup new animation
            boolAnimation = new AnimBool(false);
            boolAnimation.valueChanged.AddListener(editor.Repaint);
        }

        /// <summary>
        /// Destroys and cleans up <see cref="AnimBool"/>.
        /// </summary>
        /// <param name="editor">
        /// Editor to remove <see cref="AnimBool"/>'s events from.
        /// </param>
        /// <param name="boolAnimation">The <see cref="AnimBool"/> to clean-up.</param>
        public static void DestroyBool(UnityEditor.Editor editor, ref AnimBool boolAnimation)
        {
            if (boolAnimation != null)
            {
                boolAnimation.valueChanged.RemoveListener(editor.Repaint);
                boolAnimation = null;
            }
        }

        /// <summary>
        /// Helper method to draw enums from a limited range.
        /// </summary>
        /// <typeparam name="ENUM">Enum to draw in the editor.</typeparam>
        /// <param name="property">Property being drawn.</param>
        /// <param name="supportedEnums">
        /// List of supported enums.  The first element is treated as default.
        /// </param>
        public static void DrawEnum<ENUM>(SerializedProperty property, params ENUM[] supportedEnums) where ENUM : System.Enum
        {
            DrawEnum(property, supportedEnums, supportedEnums[0]);
        }

        /// <summary>
        /// Helper method to draw enums from a limited range.
        /// </summary>
        /// <typeparam name="ENUM">Enum to draw in the editor.</typeparam>
        /// <param name="property">Property being drawn.</param>
        /// <param name="supportedEnums">
        /// List of supported enums.  The first element is treated as default.
        /// </param>
        public static void DrawEnum<ENUM>(SerializedProperty property, ENUM[] supportedEnums, ENUM defaultEnum) where ENUM : System.Enum
        {
            // Setup the pop-up
            string[] enumNames = new string[supportedEnums.Length];
            int[] enumValues = new int[supportedEnums.Length];
            for (int index = 0; index < supportedEnums.Length; ++index)
            {
                enumNames[index] = ObjectNames.NicifyVariableName(supportedEnums[index].ToString());
                enumValues[index] = Helpers.ConvertToInt(supportedEnums[index]);
            }

            // Disable the pop-up if there's only one option
            bool wasEnabled = GUI.enabled;
            GUI.enabled = (supportedEnums.Length > 1);

            // Show the pop-up
            property.enumValueIndex = EditorGUILayout.IntPopup(property.displayName, property.enumValueIndex, enumNames, enumValues);

            // Revert the later controls
            GUI.enabled = wasEnabled;

            // Verify the selected value is within range
            ENUM selectedValue = Helpers.ConvertToEnum<ENUM>(property.enumValueIndex);
            if (ArrayUtility.Contains(supportedEnums, selectedValue) == false)
            {
                // If not, select the default option
                property.enumValueIndex = Helpers.ConvertToInt(defaultEnum);
            }
        }

        /// <summary>
        /// Helper method to draw enums from a limited range.
        /// Draws a warning if the target has an enum that doesn't match the property
        /// </summary>
        /// <typeparam name="ENUM">Enum to draw in the editor.</typeparam>
        /// <param name="property">Property being drawn.</param>
        /// <param name="supportedEnums">
        /// List of supported enums.  The first element is treated as default.
        /// </param>
        /// <param name="targetsEnum">
        /// If an enum value is not supported, this enum is used instead.
        /// </param>
        /// <param name="defaultEnum">Default value to set the enum to.</param>
        public static void DrawEnum<ENUM>(SerializedProperty property, ENUM[] supportedEnums, ENUM defaultEnum, ENUM targetsEnum, string message = "\"{0}\" is not supported; \"{1}\" will be used instead.") where ENUM : System.Enum
        {
            // Check if we need to display the warning
            int targetEnumValueIndex = Helpers.ConvertToInt(targetsEnum);
            if (property.enumValueIndex != targetEnumValueIndex)
            {
                ENUM selectedValue = Helpers.ConvertToEnum<ENUM>(property.enumValueIndex);
                string formattedMessage = string.Format(message,
                    ObjectNames.NicifyVariableName(selectedValue.ToString()),
                    ObjectNames.NicifyVariableName(targetsEnum.ToString()));
                EditorGUILayout.HelpBox(formattedMessage, MessageType.Warning);
            }

            // Draw enums as normal
            DrawEnum(property, supportedEnums, defaultEnum);
        }

        /// <summary>
        /// Draws a foldout with bold text.
        /// </summary>
        /// <param name="buildSettingsAnimation">
        /// <see cref="AnimBool"/> for playing the slide-open animation.
        /// </param>
        /// <param name="displayLabel">Text to display on foldout.</param>
        public static void DrawBoldFoldout(AnimBool buildSettingsAnimation, string displayLabel)
        {
            // Grab foldout style
            GUIStyle boldFoldoutStyle = EditorStyles.foldout;

            // Change the font to bold
            FontStyle lastFontStyle = boldFoldoutStyle.fontStyle;
            boldFoldoutStyle.fontStyle = FontStyle.Bold;

            // Draw the UI
            buildSettingsAnimation.target = EditorGUILayout.Foldout(buildSettingsAnimation.target, displayLabel, boldFoldoutStyle);

            // Revert the font
            boldFoldoutStyle.fontStyle = lastFontStyle;
        }
    }
}
