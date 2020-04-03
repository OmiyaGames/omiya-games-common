using System.Collections.Generic;

namespace OmiyaGames
{
    ///-----------------------------------------------------------------------
    /// <copyright file="ThreadSafe.cs" company="Omiya Games">
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
    /// <author>Taro Omiya</author>
    /// <date>10/2/2018</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// Creates a thread-safe variable.  Only works on primitives.
    /// Note: if a thread-safe integer is in plans, use <code>ThreadSafeInt</code> instead.
    /// </summary>
    /// <seealso cref="ThreadSafeInt"/>
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    ///   <listheader>
    ///     <description>Date</description>
    ///     <description>Name</description>
    ///     <description>Description</description>
    ///   </listheader>
    ///   <item>
    ///     <description>10/2/2018</description>
    ///     <description>Taro</description>
    ///     <description>Initial version</description>
    ///   </item>
    ///   <item>
    ///     <description>3/25/2020</description>
    ///     <description>Taro</description>
    ///     <description>Converted the class to a package</description>
    ///   </item>
    /// </list>
    /// </remarks>
    public class ThreadSafe<T>
    {
        protected T value;

        // Constructors
        public ThreadSafe() : this(default(T)) { }

        public ThreadSafe(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// The lock
        /// </summary>
        protected object ThreadLock
        {
            get;
        } = new object();

        /// <summary>
        /// Thread-safe access to value.
        /// </summary>
        public T Value
        {
            get
            {
                lock (ThreadLock)
                {
                    return value;
                }
            }
            set
            {
                lock (ThreadLock)
                {
                    this.value = value;
                }
            }
        }

        public override string ToString()
        {
            lock (ThreadLock)
            {
                return value.ToString();
            }
        }

        public override int GetHashCode()
        {
            lock (ThreadLock)
            {
                return value.GetHashCode();
            }
        }

        /// <summary>
        /// Checks the type of argument.
        /// If it's another <see cref="ThreadSafe{T}"/>, compares the two <see cref="Value"/>.
        /// If it's <typeparamref name="T"/>, compares it with <see cref="Value"/> in a thread-safe manner.
        /// </summary>
        /// <param name="obj">The object to compare to.</param>
        /// <returns>
        /// If it's another <see cref="ThreadSafe{T}"/>, returns true
        /// if two wrapper's <see cref="Value"/> matches.
        /// If it's <typeparamref name="T"/>, returns true
        /// if <see cref="Vlaue"/> matches with the argument.
        /// Otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is ThreadSafe<T> otherWrapper)
            {
                lock (ThreadLock)
                {
                    return (Comparer<T>.Default.Compare(otherWrapper.Value, this.value) == 0);
                }
            }
            else if (obj is T otherValue)
            {
                lock (ThreadLock)
                {
                    return (Comparer<T>.Default.Compare(otherValue, this.value) == 0);
                }
            }
            else
            {
                return false;
            }
        }
    }
}
