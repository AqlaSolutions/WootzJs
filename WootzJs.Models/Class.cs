﻿#region License

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace WootzJs.Models
{
    public static class Class<T>
    {
        private static Dictionary<string, PropertyInfo> propertiesByName = typeof(T)
            .GetProperties()
            .ToDictionary(x => x.Name);

        public static PropertyInfo GetPropertyInfo(string name)
        {
            return propertiesByName[name];
        }

        public static MethodInfo GetMethodInfo(Expression<Action<T>> accessor)
        {
            var call = (MethodCallExpression)accessor.Body;
            return call.Method;
        }

        public static MethodInfo GetMethodInfo<TResult>(Expression<Func<T, TResult>> accessor)
        {
            var call = (MethodCallExpression)accessor.Body;
            return call.Method;
        }

        public static MemberInfo GetMemberInfo<TResult>(Expression<Func<T, TResult>> accessor)
        {
            var call = (MemberExpression)accessor.Body;
            return call.Member;
        }

        public static PropertyInfo GetPropertyInfo<TResult>(Expression<Func<T, TResult>> accessor)
        {
            var expression = accessor.Body;
            if (expression is UnaryExpression)
                expression = ((UnaryExpression)expression).Operand;
            var call = (MemberExpression)expression;
            return (PropertyInfo)call.Member;
        }
    }

    public static class Class
    {
        private static IEnumerable<T> SelectRecursive<T>(this T obj, Func<T, T> next) where T : class
        {
            T current = obj;
            while (current != null)
            {
                yield return current;
                current = next(current);
            }
        }

        public static string GetPropertyName(this LambdaExpression expression)
        {
            var current = expression.Body;
            var unary = current as UnaryExpression;
            if (unary != null)
                current = unary.Operand;
            var member = (MemberExpression)current;
            return string.Join(".", member
                .SelectRecursive(o => o.Expression as MemberExpression)
                .Select(o => o.Member.Name)
                .Reverse());
        }

        public static PropertyInfo GetPropertyInfo(this LambdaExpression expression)
        {
            var current = expression.Body;
            var unary = current as UnaryExpression;
            if (unary != null)
                current = unary.Operand;
            var call = current as MemberExpression;
            return (PropertyInfo)call?.Member;
        }

        public static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            var current = expression.Body;
            var unary = current as UnaryExpression;
            if (unary != null)
                current = unary.Operand;
            var call = current as MemberExpression;
            if (call != null)
                return call.Member;

            var method = current as MethodCallExpression;
            return method?.Method;
        }

        public static IEnumerable<MemberInfo> GetPropertyPath(this LambdaExpression expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null) 
                return Enumerable.Empty<MemberInfo>();
            return member
                .SelectRecursive(o => o.Expression as MemberExpression)
                .Select(o => o.Member).Reverse();
        }

        public static Action<T, TValue> GenerateSetter<T, TValue>(this Expression<Func<T, TValue>> property)
        {
            var propertyPath = property.GetPropertyPath().ToArray();
            return (x, value) =>
            {
                object current = x;
                for (var i = 0; i < propertyPath.Length - 1; i++)
                {
                    var currentProperty = (PropertyInfo)propertyPath[i];
                    current = currentProperty.GetValue(current);
                }
                var finalProperty = (PropertyInfo)propertyPath.Last();
                finalProperty.SetValue(current, value);
            };
        }


        private static HashSet<Type> numericTypes = new HashSet<Type>(new[]
        {
            typeof(int), typeof(float), typeof(double), typeof(byte), typeof(sbyte), typeof(short), typeof(ushort),
            typeof(uint), typeof(long), typeof(ulong)
        });

        public static bool IsNumeric(this Type type)
        {
            return numericTypes.Contains(type);
        }
    }
}