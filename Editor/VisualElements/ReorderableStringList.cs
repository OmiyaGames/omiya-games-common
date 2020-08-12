using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace OmiyaGames.Common.Editor
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="IReorderableList.cs" company="Omiya Games">
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
    /// <strong>Version:</strong> TODO<br/>
    /// <strong>Date:</strong> 8/10/2020<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>Initial verison.</description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// TODO
    /// </summary>
    public class ReorderableStringList : IReorderableList<string>
    {
        /// <inheritdoc/>
        protected override void DrawDomainElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.y += EditorHelpers.VerticalMargin;
            rect.height = EditorGUIUtility.singleLineHeight;
            value[index] = EditorGUI.TextField(rect, value[index]);

            // TODO: consider syncing with serialization here
        }

        /// <inheritdoc/>
        protected override float GetElementHeight(int index)
        {
            return EditorHelpers.SingleLineHeight(EditorHelpers.VerticalMargin);
        }

        public new class UxmlFactory : UxmlFactory<ReorderableStringList, UxmlTraits> { }

        public new class UxmlTraits : IReorderableList<string>.UxmlTraits { }
    }


    // FIXME: just esting, remove the next couple of lines
    public class TestField<T> : BaseField<IList<T>>
    {
        protected TestField(string label, VisualElement visualInput) : base(label, visualInput)
        {
        }
    }
}
