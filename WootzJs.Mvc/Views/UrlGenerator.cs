﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.WootzJs;
using System.Text;
using WootzJs.Mvc.Utils;

namespace WootzJs.Mvc.Views
{
    public class UrlGenerator
    {
        public static string GenerateUrl(LambdaExpression action)
        {
            var result = new StringBuilder();

            var methodCallExpression = (MethodCallExpression)action.Body;
            var method = methodCallExpression.Method;
            var controllerDefaultAttribute = (DefaultAttribute)Attribute.GetCustomAttribute(method.DeclaringType, typeof(DefaultAttribute), false);
            var routeAttribute = (RouteAttribute)Attribute.GetCustomAttribute(method, typeof(RouteAttribute), false);
            var defaultAttribute = (DefaultAttribute)Attribute.GetCustomAttribute(method, typeof(DefaultAttribute), false);
            var args = methodCallExpression.ExtractArguments();
            if (controllerDefaultAttribute != null && defaultAttribute != null)
                result.Append("/");
            else if (defaultAttribute != null)
                result.Append("/" + GetControllerNameFromType(method.DeclaringType));
            else if (routeAttribute != null && routeAttribute.Value != null)
            {
                result.Append(GenerateUrlFromTemplate(routeAttribute.Value, args));
            }
            else 
                result.Append(GenerateUrlFromMethod(method));

            bool hasAddedArgument = false;
            foreach (var argument in args)
            {
                if (!hasAddedArgument)
                    result.Append("?");
                else
                    result.Append("&");
                hasAddedArgument = true;
                result.Append(argument.Key);
                result.Append("=");
                result.Append(argument.Value);
            }

            return result.ToString();
        }

        private static Dictionary<string, BraceTokenizer.IToken[]> tokensCache = new Dictionary<string, BraceTokenizer.IToken[]>();

        private static string GenerateUrlFromTemplate(string template, Dictionary<string, object> routeValues)
        {
            var tokens = tokensCache.Get(template);
            if (tokens == null)
            {
                tokens = template.BraceTokenize().ToArray();
                tokensCache[template] = tokens;
            }
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token is BraceTokenizer.Literal)
                {
                    builder.Append(token.Literal);
                }
                else
                {
                    var id = token.VariableId;
                    var value = routeValues.Get(id);
                    builder.Append(value);
                    routeValues.Remove(id);
                }
            }
            return builder.ToString();
        }

        private static string GenerateUrlFromMethod(MethodInfo method)
        {
            var controller = GetControllerNameFromType(method.DeclaringType);
            var action = method.Name.ToLower();

            return "/" + controller + "/" + action;
        }

        private static string GetControllerNameFromType(Type type)
        {
            return type.Name.Substring(0, type.Name.Length - "Controller".Length).ToLower();
        }
    }
}