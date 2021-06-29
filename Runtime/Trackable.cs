using UnityEngine;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <remarks>
    /// <copyright file="RandomList.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2014-2021 Omiya Games
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
    /// <strong>Date:</strong> 6/28/2021<br/>
    /// <strong>Author:</strong> Taro Omiya
    /// </term>
    /// <description>
    /// Initial version.
    /// </description>
    /// </item>
    /// </list>
    /// </remarks>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// A <see cref="System.Serializable"/> value one can track changes to by
    /// using 
    /// </summary>
    [System.Serializable]
    public class Trackable<T>
    {
        [SerializeField]
        T value;

        /// <summary>
        /// Event called before the value has changed.
        /// Will be called even if the new value is the same as old.
        /// </summary>
        public Helpers.ChangeEvent<Trackable<T>, T> OnBeforeValueChanged;
        /// <summary>
        /// Event called after the value has changed.
        /// Will be called even if the new value is the same as old.
        /// </summary>
        public Helpers.ChangeEvent<Trackable<T>, T> OnAfterValueChanged;

        /// <summary>
        /// The value this class represents.
        /// </summary>
        public T Value
        {
            get => value;
            set
            {
                OnBeforeValueChanged?.Invoke(this, this.value, value);
                T oldValue = this.value;
                this.value = value;
                OnAfterValueChanged?.Invoke(this, oldValue, this.value);
            }
        }
    }
}
