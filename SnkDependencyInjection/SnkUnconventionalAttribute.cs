using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 该属性用于标记类，并且不能多次应用在同一个类上
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SnkUnconventionalAttribute : Attribute
    {
    }
}
