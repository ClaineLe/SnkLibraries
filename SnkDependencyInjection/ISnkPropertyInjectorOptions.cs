namespace SnkDependencyInjection
{
    /// <summary>
    /// 定义属性注入器选项的接口。
    /// 提供配置属性注入行为的选项。
    /// </summary>
    public interface ISnkPropertyInjectorOptions
    {
        /// <summary>
        /// 获取注入属性的策略。
        /// </summary>
        SnkPropertyInjection InjectIntoProperties { get; }

        /// <summary>
        /// 获取一个值，指示在属性注入失败时是否应该抛出异常。
        /// </summary>
        bool ThrowIfPropertyInjectionFails { get; }
    }
}