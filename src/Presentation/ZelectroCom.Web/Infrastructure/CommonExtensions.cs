using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using BForms.Models;

namespace ZelectroCom.Web.Infrastructure
{
    public static class CommonExtensions
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType().GetMember(enumValue.ToString())
                           .First()
                           .GetCustomAttribute<DisplayAttribute>()
                           .Name;
        }

        public static string GetPropertyName<T, TProp>(this T instance, Expression<Func<T, TProp>> selector)
        {
            return GetPropertyName(selector);
        }

        public static string GetPropertyName<T, TProp>(Expression<Func<T, TProp>> selector)
        {
            var member = selector.Body as MemberExpression;
            var unary = selector.Body as UnaryExpression;
            var memberInfo = member ?? (unary != null ? unary.Operand as MemberExpression : null);
            if (memberInfo == null)
            {
                throw new Exception("Could not get selector from specified expression.");
            }
            return memberInfo.Member.Name;
        }
    }

    public static class HtmlExtensions
    {
        public static BsTheme GetTheme(this HtmlHelper html)
        {
            return BsTheme.Green;
        }
    }
}