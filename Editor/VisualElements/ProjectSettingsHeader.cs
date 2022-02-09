using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="ProjectSettingsHeader.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020-2020 Omiya Games
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
    /// <strong>Version:</strong> 0.2.0-preview.1<br/>
    /// <strong>Date:</strong> 9/27/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial verison.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// The header label to the project settings dialog.
    /// </summary>
    public class ProjectSettingsHeader : TextElement
    {
        /// <summary>
        /// <seealso cref="UxmlFactory{TCreatedType, TTraits}"/> for <see cref="ProjectSettingsHeader"/>.
        /// </summary>
        public new class UxmlFactory : UxmlFactory<ProjectSettingsHeader, UxmlTraits> { }

        /// <summary>
        /// <seealso cref="UxmlTraits"/> for <see cref="ProjectSettingsHeader"/>.
        /// </summary>
        public new class UxmlTraits : TextElement.UxmlTraits
        {
            /// <summary>
            /// Attribute corresponding to <c>help-url</c>.
            /// </summary>
            protected UxmlStringAttributeDescription helpUrlAttr = new UxmlStringAttributeDescription
            {
                name = "help-url",
                defaultValue = null
            };

            /// <inheritdoc/>
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);

                if (ve is ProjectSettingsHeader)
                {
                    ProjectSettingsHeader header = (ProjectSettingsHeader)ve;
                    header.HelpUrl = helpUrlAttr.GetValueFromBag(bag, cc);

                    // Empty the content
                    header.contentContainer.Clear();

                    //// Check if there are any items
                    //if (string.IsNullOrEmpty(header.HelpUrl) == false)
                    //{
                    //    header.contentContainer.Add(header.HelpButton);
                    //}
                }
            }
        }

        /// <summary>
        /// Constructs an empty <see cref="VisualElement"/> with
        /// the height of <see cref="DefaultHeight"/>.
        /// </summary>
        public ProjectSettingsHeader()
        {
            // Setup default font styles
            style.unityFontStyleAndWeight = new StyleEnum<FontStyle>(FontStyle.Bold);
            style.fontSize = new Length(19, LengthUnit.Pixel);

            // Setup default margins
            style.marginTop = new Length(2, LengthUnit.Pixel);
            style.marginLeft = new Length(10, LengthUnit.Pixel);

            // Setup the help button
            HelpButton = new Button(OpenHelpUrl);
            HelpButton.style.position = new StyleEnum<Position>(Position.Absolute);
            HelpButton.style.right = new Length(0, LengthUnit.Pixel);
            HelpButton.style.width = new Length(16, LengthUnit.Pixel);

            //// Grab the help icon
            //Image image = new Image();
            //GUIContent helpIcon = EditorGUIUtility.IconContent("_Help");
            //image.image = helpIcon.image;
            //HelpButton.contentContainer.Add(image);
        }

        /// <inheritdoc/>
        public override bool canGrabFocus => false;

        /// <summary>
        /// The URL that the help button opens.
        /// </summary>
        public string HelpUrl
        {
            get;
            set;
        } = null;

        /// <summary>
        /// Corresponding control for the help button.
        /// </summary>
        public Button HelpButton
        {
            get;
        }

        /// <summary>
        /// Opens the web browser to open <see cref="HelpUrl"/>.
        /// </summary>
        private void OpenHelpUrl()
        {
            if (string.IsNullOrEmpty(HelpUrl) == false)
            {
                Application.OpenURL(HelpUrl);
            }
        }
    }
}
