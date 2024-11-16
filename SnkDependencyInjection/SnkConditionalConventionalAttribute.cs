using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 一个抽象的属性条件化特性类，适用于属性标记，表示条件满足时才进行某些操作（如属性注入）。
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public abstract class SnkConditionalConventionalAttribute : Attribute
    {
        /// <summary>
        /// 获取一个布尔值，表示当前条件是否满足。
        /// 该属性必须是抽象的，以便每个具体实现可以定义自己的条件检查逻辑。
        /// </summary>
        public abstract bool IsConditionSatisfied { get; }
    }
}