#region License
//-----------------------------------------------------------------------
// <copyright>
// The MIT License (MIT)
// 
// Copyright (c) 2014 Kirk S Woll
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
//-----------------------------------------------------------------------
#endregion

namespace System.Collections
{
    /// <summary>
    /// Defines methods to support the comparison of objects for structural equality.
    /// </summary>
    public interface IStructuralEquatable
    {
        /// <summary>
        /// Determines whether an object is structurally equal to the current instance.
        /// </summary>
        /// 
        /// <returns>
        /// true if the two objects are equal; otherwise, false.
        /// </returns>
        /// <param name="other">The object to compare with the current instance.</param><param name="comparer">An object that determines whether the current instance and <paramref name="other"/> are equal. </param>
        bool Equals(object other, IEqualityComparer comparer);

        /// <summary>
        /// Returns a hash code for the current instance.
        /// </summary>
        /// 
        /// <returns>
        /// The hash code for the current instance.
        /// </returns>
        /// <param name="comparer">An object that computes the hash code of the current object.</param>
        int GetHashCode(IEqualityComparer comparer);
    }
}
