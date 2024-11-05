using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 表示一个属性应通过依赖注入进行赋值的特性。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class SnkInjectAttribute : Attribute
    {
    }
}