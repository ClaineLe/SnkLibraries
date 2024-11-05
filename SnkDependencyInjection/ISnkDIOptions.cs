using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 定义依赖注入选项的接口。
    /// </summary>
    public interface ISnkDIOptions
    {
        /// <summary>
        /// 获取是否检测单例模式下的循环引用，防止因循环依赖导致的内存泄漏或死锁。
        /// </summary>
        bool TryToDetectSingletonCircularReferences { get; }

        /// <summary>
        /// 获取是否检测动态对象的循环引用，确保动态注入的对象不会形成循环依赖。
        /// </summary>
        bool TryToDetectDynamicCircularReferences { get; }

        /// <summary>
        /// 获取当属性注入失败时，是否检查并尝试释放已分配的资源，防止资源泄漏。
        /// </summary>
        bool CheckDisposeIfPropertyInjectionFails { get; }

        /// <summary>
        /// 获取指定用于属性注入的注入器类型，允许自定义属性注入逻辑。
        /// </summary>
        Type PropertyInjectorType { get; }

        /// <summary>
        /// 获取属性注入器的配置选项，用于精细化控制属性注入的行为。
        /// </summary>
        ISnkPropertyInjectorOptions PropertyInjectorOptions { get; }
    }
}