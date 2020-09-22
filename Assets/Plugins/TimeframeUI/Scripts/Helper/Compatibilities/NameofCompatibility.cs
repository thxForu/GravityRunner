namespace Termway.Helper
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// Backward compativibity for nameof (pre C# 6) 
    /// Proposed usage : 
    /// Name.Of(() => Type.Member);   => "Member"
    /// Name.Of(Type.Method);   => "Method"
    /// Name.Of<Class>();   => "Class"
    /// </summary>
    public static class Name
    {
        /// <summary>
        /// Usage Name.Of(() => member); => "member"
        /// </summary>
        public static string Of<T>(Expression<Func<T>> member)
        {
            return (member.Body as MemberExpression).Member.Name;
        }

        /// <summary>
        /// Usage : Name.Of((Type t) => t.member); => "member"
        /// Usage : Name.Of<Type, Tmember>(t => t.member); => "member"
        /// </summary>
        public static string Of<Tin, Tout>(Expression<Func<Tin, Tout>> member)
        {
            return (member.Body as MemberExpression).Member.Name;
        }

        /// <summary>
        /// Usage Name.Of<Class>(); => "Class"
        /// </summary>
        public static string Of<T>()
        {
            return typeof(T).Name;
        }
    }
}