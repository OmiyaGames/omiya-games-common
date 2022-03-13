using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace OmiyaGames.Common.Editor
{
	///-----------------------------------------------------------------------
	/// <remarks>
	/// <copyright file="RangeSlider.cs" company="Omiya Games">
	/// The MIT License (MIT)
	/// 
	/// Copyright (c) 2022 Omiya Games
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
	/// <strong>Version:</strong> 1.3.0<br/>
	/// <strong>Date:</strong> 3/13/2022<br/>
	/// <strong>Author:</strong> Taro Omiya
	/// </term>
	/// <description>Initial verison.</description>
	/// </item>
	/// </list>
	/// </remarks>
	///-----------------------------------------------------------------------
	/// <summary>
	/// A min/max slider that further adds <see cref="FloatField"/>s to
	/// fine-tune the min and max range values.
	/// </summary>
	public class RangeSlider : MinMaxSlider
	{
		const float FIELD_FLEX_GROW = 0.3f;
		const float MIN_FIELD_WIDTH = 20f;
		const float FIELD_PADDING = 8f;
		/// <summary>
		/// TODO
		/// </summary>
		public static readonly string minFieldUssClassName = ussClassName + "__min-field";
		/// <summary>
		/// TODO
		/// </summary>
		public static readonly string maxFieldUssClassName = ussClassName + "__max-field";

		/// <summary>
		/// <seealso cref="UxmlFactory{TCreatedType, TTraits}"/> for <see cref="RangeSlider"/>.
		/// </summary>
		public new class UxmlFactory : UxmlFactory<RangeSlider, UxmlTraits> { }

		FloatField minField
		{
			get;
		}
		FloatField maxField
		{
			get;
		}

		/// <summary>
		/// TODO
		/// </summary>
		public RangeSlider() : this(null) { }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <param name="minLimit"></param>
		/// <param name="maxLimit"></param>
		public RangeSlider(float minValue, float maxValue, float minLimit, float maxLimit) : this(null, minValue, maxValue, minLimit, maxLimit) { }

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="label"></param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <param name="minLimit"></param>
		/// <param name="maxLimit"></param>
		public RangeSlider(string label, float minValue = 0, float maxValue = 10, float minLimit = -10, float maxLimit = 20) : base(label, minValue, maxValue, minLimit, maxLimit)
		{
			// Setup the float fields
			minField = new FloatField()
			{
				name = "min-field"
			};
			maxField = new FloatField()
			{
				name = "max-field"
			};

			// Add classes
			minField.AddToClassList(minFieldUssClassName);
			maxField.AddToClassList(maxFieldUssClassName);

			// Setup field styles
			// TODO: verify if this can be overwritten by classes
			minField.style.flexBasis = 0f;
			minField.style.flexGrow = FIELD_FLEX_GROW;
			minField.style.minWidth = MIN_FIELD_WIDTH;
			minField.style.paddingRight = FIELD_PADDING;
			maxField.style.flexBasis = 0f;
			maxField.style.flexGrow = FIELD_FLEX_GROW;
			maxField.style.minWidth = MIN_FIELD_WIDTH;
			maxField.style.paddingLeft = FIELD_PADDING;

			// Setup their initial values
			minField.value = minValue;
			maxField.value = maxValue;

			// Bind events
			minField.RegisterCallback<ChangeEvent<float>>(e => base.minValue = e.newValue);
			maxField.RegisterCallback<ChangeEvent<float>>(e => base.maxValue = e.newValue);

			// Insert the fields
			contentContainer.Insert(0, minField);
			contentContainer.Add(maxField);
		}

		/// <inheritdoc/>
		public override Vector2 value
		{
			get => base.value;
			set
			{
				// Update slider
				base.value = value;

				// Update the text fields
				minField?.SetValueWithoutNotify(base.value.x);
				maxField?.SetValueWithoutNotify(base.value.y);
			}
		}

		/// <inheritdoc/>
		public override void SetValueWithoutNotify(Vector2 newValue)
		{
			// Update slider
			base.SetValueWithoutNotify(newValue);

			// Update the text fields
			minField?.SetValueWithoutNotify(newValue.x);
			maxField?.SetValueWithoutNotify(newValue.y);
		}
	}
}
