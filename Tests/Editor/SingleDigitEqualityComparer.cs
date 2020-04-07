using System.Collections.Generic;

namespace OmiyaGames.Common.Runtime.Tests
{
    ///-----------------------------------------------------------------------
    /// <copyright file="SingleDigitEqualityComparer.cs" company="Omiya Games">
    /// The MIT License (MIT)
    /// 
    /// Copyright (c) 2020 Omiya Games
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
    /// <date>4/7/2020</date>
    ///-----------------------------------------------------------------------
    /// <summary>
    /// An <see cref="IEqualityComparer{T}"/> that compares only the first digit
    /// of two integers. Intended for unit-testing sets and dictionaries.
    /// </summary>
    /// <remarks>
    /// Revision History:
    /// <list type="table">
    /// <listheader>
    ///   <description>Date</description>
    ///   <description>Author</description>
    ///   <description>Description</description>
    /// </listheader>
    /// <item>
    ///   <description>4/7/2020</description>
    ///   <description>Taro</description>
    ///   <description>Initial verison</description>
    /// </item>
    /// </list>
    /// </remarks>
    public class SingleDigitEqualityComparer : IEqualityComparer<int>
    {
        public static int GetSingleDigit(int x)
        {
            return x % 10;
        }

        public bool Equals(int x, int y)
        {
            return GetSingleDigit(x) == GetSingleDigit(y);
        }

        public int GetHashCode(int obj)
        {
            return GetSingleDigit(obj).GetHashCode();
        }
    }
}
