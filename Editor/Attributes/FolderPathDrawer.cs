using UnityEngine;
using UnityEditor;
using System.IO;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="FolderPathDrawer.cs" company="Omiya Games">
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
    /// <strong>Date:</strong> 11/01/2018<br/>
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
    /// <see cref="PropertyDrawer"/> for <see cref="FolderPathAttribute"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathDrawer : PropertyDrawer
    {
        const float messageHeight = 36f;
        const float buttonWidth = 63f;

        /// <summary>
        /// Converts a full path into a relative path.
        /// </summary>
        /// <param name="fullPath"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        public static string GetLocalPath(string fullPath, FolderPathAttribute.RelativeTo relativeTo)
        {
            if (relativeTo == FolderPathAttribute.RelativeTo.ProjectDirectory)
            {
                int startLocalPath = fullPath.IndexOf(FolderPathAttribute.DefaultLocalPath);
                if (startLocalPath > 0)
                {
                    fullPath = fullPath.Substring(startLocalPath);
                }
            }
            return fullPath;
        }

        /// <inheritdoc/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float singleLineHeight = base.GetPropertyHeight(property, label);
            if ((IsValid == true) && (IsMessageBoxShown(property, attribute as FolderPathAttribute) == true))
            {
                singleLineHeight += messageHeight;
            }
            return singleLineHeight;
        }

        // Draw the property inside the given rect
        /// <inheritdoc/>
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // First get the attribute since it contains the range for the slider
            if (IsValid == true)
            {
                // Draw the property with a string field and a button.
                if (property.propertyType == SerializedPropertyType.String)
                {
                    Rect textPosition, buttonPosition;
                    bool showMessageBox = CalculatePositions(position, property, out textPosition, out buttonPosition);

                    // Label
                    EditorGUI.BeginProperty(position, label, property);

                    // Show button
                    if (GUI.Button(buttonPosition, "Browse...") == true)
                    {
                        OpenDialog(property, label);
                    }

                    // Show text field
                    property.stringValue = EditorGUI.TextField(textPosition, label, property.stringValue);

                    // Draw message box
                    if (showMessageBox)
                    {
                        // Draw the message box
                        position.height -= textPosition.height;
                        position.height -= EditorHelpers.VerticalMargin;
                        EditorGUI.HelpBox(position, WrongPathMessage, MessageType.Error);
                    }
                    EditorGUI.EndProperty();
                }
                else
                {
                    EditorGUI.LabelField(position, label.text, WrongAttributeMessage);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual bool IsValid
        {
            get
            {
                return attribute is FolderPathAttribute;
            }
        }

        /// <summary>
        /// Message for entering the wrong path.
        /// </summary>
        public string WrongPathMessage
        {
            get
            {
                return "Invalid Path";
            }
        }

        /// <summary>
        /// Message for using the attribute on the wrong variable type.
        /// </summary>
        public virtual string WrongAttributeMessage
        {
            get
            {
                return "Use FolderPath attribute with a string.";
            }
        }

        /// <summary>
        /// Indicates if an error message box is shown.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public virtual bool IsMessageBoxShown(SerializedProperty property, FolderPathAttribute attribute)
        {
            bool showMessage = false;
            if ((attribute != null) && (attribute.IsWarningDisplayed == true))
            {
                // FIXME: check local path
                showMessage = (Directory.Exists(property.stringValue) == false);
            }
            return showMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="textPosition"></param>
        /// <param name="buttonPosition"></param>
        /// <returns></returns>
        private bool CalculatePositions(Rect position, SerializedProperty property, out Rect textPosition, out Rect buttonPosition)
        {
            // Calculate text positioning
            textPosition = position;
            textPosition.width -= buttonWidth + EditorHelpers.VerticalMargin;

            // Calculate button positioning
            buttonPosition = position;
            buttonPosition.x += textPosition.width;
            buttonPosition.x += EditorHelpers.VerticalMargin;
            buttonPosition.width = buttonWidth;

            // Draw message box
            bool showMessageBox = IsMessageBoxShown(property, attribute as FolderPathAttribute);
            if (showMessageBox)
            {
                // Calculate text positioning
                textPosition.y += textPosition.height;
                textPosition.height = EditorGUIUtility.singleLineHeight;
                textPosition.y -= textPosition.height;

                // Calculate button positioning
                buttonPosition.y = textPosition.y;
                buttonPosition.height = textPosition.height;
            }
            return showMessageBox;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        /// <param name="label"></param>
        protected virtual void OpenDialog(SerializedProperty property, GUIContent label)
        {
            // Open a folder panel
            FolderPathAttribute path = (FolderPathAttribute)attribute;
            string browsedFolder = EditorUtility.OpenFolderPanel(label.text, path.DefaultPath, null);

            // Check if a folder was found
            if (string.IsNullOrEmpty(browsedFolder) == false)
            {
                property.stringValue = GetLocalPath(browsedFolder, path.PathRelativeTo);
            }
        }
    }
}
