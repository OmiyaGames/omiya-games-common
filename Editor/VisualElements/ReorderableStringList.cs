using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace OmiyaGames.Common.Editor
{
    public class ReorderableStringList : IReorderableList<string>
    {
        public string stringAttr { get; set; }

        public new class UxmlFactory : UxmlFactory<ReorderableStringList, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlStringAttributeDescription m_String = new UxmlStringAttributeDescription { name = "string-attr", defaultValue = "default_value" };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                ReorderableStringList ate = (ReorderableStringList)ve;

                ate.Clear();

                ate.stringAttr = m_String.GetValueFromBag(bag, cc);
                ate.Add(new TextField("String") { value = ate.stringAttr });
            }
        }
    }
}
