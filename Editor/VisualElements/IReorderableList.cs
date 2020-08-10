using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UIElements;

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
    public abstract class IReorderableList<T> : IMGUIContainer, IBindable, INotifyValueChanged<IList<T>>
    {
        /// <summary>
        /// TODO
        /// </summary>
        private IList<T> bindedList = new T[0];
        /// <summary>
        /// TODO
        /// </summary>
        protected readonly ReorderableList drawnList;
        /// <summary>
        /// TODO
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// TODO
        /// </summary>
        public bool IsExpanded { get; set; } = true;
        /// <inheritdoc/>
        public IBinding binding
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
        /// <inheritdoc/>
        public string bindingPath
        {
            get => throw new System.NotImplementedException();
            set => throw new System.NotImplementedException();
        }
        /// <inheritdoc/>
        public IList<T> value
        {
            get => bindedList;
            set
            {
                SetValueWithoutNotify(value);
                drawnList.list = (IList)bindedList;
                MarkDirtyLayout();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public IReorderableList()
        {
            // Setup domainMustContain list
            drawnList = new ReorderableList((IList)value, typeof(T), true, true, true, true);
            drawnList.drawHeaderCallback = DrawDomainHeader;
            drawnList.drawElementCallback = DrawDomainElement;
            drawnList.elementHeightCallback = GetElementHeight;

            // TODO: consider creating an actual custom UXML tag than using an IMGUIContainer
            onGUIHandler += DrawReorderableList;
        }

        /// <summary>
        /// TODO
        /// </summary>
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            /// <summary>
            /// TODO
            /// </summary>
            protected UxmlStringAttributeDescription text = new UxmlStringAttributeDescription { name = "text", defaultValue = "" };
            /// <summary>
            /// TODO
            /// </summary>
            protected UxmlBoolAttributeDescription expanded = new UxmlBoolAttributeDescription { name = "expanded", defaultValue = true };

            /// <inheritdoc/>
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                IReorderableList<T> ate = (IReorderableList<T>)ve;
                ate.Text = text.GetValueFromBag(bag, cc);
                ate.IsExpanded = expanded.GetValueFromBag(bag, cc);
            }
        }

        /// <inheritdoc/>
        public void SetValueWithoutNotify(IList<T> newValue)
        {
            bindedList = newValue;
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="index"></param>
        /// <param name="isActive"></param>
        /// <param name="isFocused"></param>
        protected abstract void DrawDomainElement(Rect rect, int index, bool isActive, bool isFocused);

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected abstract float GetElementHeight(int index);

        /// <summary>
        /// TODO
        /// </summary>
        protected virtual void DrawReorderableList()
        {
            if(IsExpanded == true)
            {
                drawnList.DoLayoutList();
                //drawnList.DoList(paddingRect);
            }
            else
            {
                IsExpanded = EditorGUILayout.Foldout(IsExpanded, Text);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="rect"></param>
        protected virtual void DrawDomainHeader(Rect rect)
        {
            IsExpanded = EditorGUI.Foldout(rect, IsExpanded, Text);
        }
    }
}
