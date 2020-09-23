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
    /// <copyright file="BaseReorderableList.cs" company="Omiya Games">
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
    public abstract class BaseReorderableList<T> : BaseField<IList<T>>//IMGUIContainer, IBindable, INotifyValueChanged<IList<T>>
    {
        /// <summary>
        /// Temporary placeholder list to display
        /// </summary>
        private static readonly IList<T> EmptyList = new T[0];
        /// <summary>
        /// TODO
        /// </summary>
        private readonly IMGUIContainer listDrawingElement;
        /// <summary>
        /// TODO
        /// </summary>
        protected readonly ReorderableList drawnList;
        /// <summary>
        /// TODO
        /// </summary>
        public string Text { get; set; } = null;
        /// <summary>
        /// TODO
        /// </summary>
        public bool IsExpanded { get; set; } = true;

        /// <summary>
        /// TODO
        /// </summary>
        public BaseReorderableList(string label, VisualElement visualInput) : base(label, visualInput)
        {
            // Setup domainMustContain list
            drawnList = new ReorderableList(((IList)EmptyList), typeof(T), true, true, true, true);
            drawnList.drawHeaderCallback = DrawDomainHeader;
            drawnList.drawElementCallback = DrawDomainElement;
            drawnList.elementHeightCallback = GetElementHeight;

            // TODO: consider creating an actual custom UXML tag than using an IMGUIContainer
            listDrawingElement = new IMGUIContainer(DrawReorderableList);
        }

        /// <summary>
        /// FIXME: this constructor likely does not work.
        /// </summary>
        public BaseReorderableList() : this(null, null) { }

        /// <inheritdoc/>
        public override void SetValueWithoutNotify(IList<T> newValue)
        {
            base.SetValueWithoutNotify(newValue);
            drawnList.index = 0;
            drawnList.list = (IList)newValue;
        }

        /// <inheritdoc/>
        public override IList<T> value
        {
            get => base.value;
            set
            {
                base.value = value;
                drawnList.index = 0;
                drawnList.list = (IList)value;
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            /// <summary>
            /// TODO
            /// </summary>
            protected UxmlStringAttributeDescription text = new UxmlStringAttributeDescription { name = "text", defaultValue = null };
            /// <summary>
            /// TODO
            /// </summary>
            protected UxmlBoolAttributeDescription expanded = new UxmlBoolAttributeDescription { name = "expanded", defaultValue = true };

            /// <inheritdoc/>
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                if (ve is BaseReorderableList<T>)
                {
                    BaseReorderableList<T> list = (BaseReorderableList<T>)ve;
                    list.IsExpanded = expanded.GetValueFromBag(bag, cc);
                    list.Text = text.GetValueFromBag(bag, cc);

                    // Force the element to contain only one child
                    list.contentContainer.Clear();
                    list.contentContainer.Add(list.listDrawingElement);
                    //list.listDrawingElement.StretchToParentWidth();
                }
            }
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
            EditorGUILayout.HelpBox("Blah, blah, blah!", MessageType.None);
            drawnList.DoLayoutList();
            //if (IsExpanded == true)
            //{
            //    drawnList.DoLayoutList();
            //}
            //else
            //{
            //    IsExpanded = EditorGUILayout.Foldout(IsExpanded, label, IsExpanded);
            //}
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="rect"></param>
        protected virtual void DrawDomainHeader(Rect rect)
        {
            //IsExpanded = EditorGUILayout.Foldout(IsExpanded, label, IsExpanded);
            EditorGUI.PrefixLabel(rect, new GUIContent(Text));
        }
    }
}
