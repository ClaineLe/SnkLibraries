using System;

namespace SnkDependencyInjection
{
    /// <summary>
    /// 依赖注入系统的配置选项。
    /// </summary>
    public class SnkDIOptions : ISnkDIOptions
    {
        /// <summary>
        /// 初始化 <see cref="SnkDIOptions"/> 类的新实例，并设置默认值。
        /// </summary>
        public SnkDIOptions()
        {
            // 默认情况下，尝试检测单例作用域中的循环引用。
            TryToDetectSingletonCircularReferences = true;

            // 默认情况下，尝试检测动态作用域中的循环引用。
            TryToDetectDynamicCircularReferences = true;

            // 默认情况下，如果属性注入失败，则检查并可能释放对象。
            CheckDisposeIfPropertyInjectionFails = true;

            // 默认情况下，使用 SnkPropertyInjector 作为属性注入器的类型。
            PropertyInjectorType = typeof(SnkPropertyInjector);

            // 默认情况下，使用新的 SnkPropertyInjectorOptions 实例作为属性注入器的选项。
            PropertyInjectorOptions = new SnkPropertyInjectorOptions();
        }

        /// <summary>
        /// 获取或设置一个值，指示是否尝试检测单例作用域中的循环引用。(默认值：true)
        /// </summary>
        public bool TryToDetectSingletonCircularReferences { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示是否尝试检测动态作用域中的循环引用。(默认值：true)
        /// </summary>
        public bool TryToDetectDynamicCircularReferences { get; set; }

        /// <summary>
        /// 获取或设置一个值，指示如果属性注入失败，是否检查并可能释放对象。(默认值：true)
        /// </summary>
        public bool CheckDisposeIfPropertyInjectionFails { get; set; }

        /// <summary>
        /// 获取或设置要使用的属性注入器的类型。(默认值：<see cref="SnkDIOptions"/>)
        /// </summary>
        public Type PropertyInjectorType { get; set; }

        /// <summary>
        /// 获取或设置属性注入器的选项。(默认值：<see cref="SnkPropertyInjectorOptions"/>)
        /// </summary>
        public ISnkPropertyInjectorOptions PropertyInjectorOptions { get; set; }
    }
}